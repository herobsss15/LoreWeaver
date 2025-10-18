from __future__ import annotations

import time
import uuid
from dataclasses import dataclass
from typing import Any, Dict, List, Optional, Sequence, Tuple

from src.app.infrastructure.cache.memory_cache import MemoryCache
from src.app.infrastructure.repositories.srd_client import (
    DOMAIN_BASE_PATH,
    SrdClient,
    SrdRule,
    hash_query,
)

CACHE_TTL_SECONDS = 6 * 60 * 60
CACHE_STALE_SECONDS = 5 * 60


@dataclass
class WarningMessage:
    code: str
    message: str
    details: Optional[Dict[str, Any]] = None


class RulesService:
    def __init__(self, srd_client: SrdClient, cache: MemoryCache) -> None:
        self._srd_client = srd_client
        self._cache = cache

    def normalize_slug(self, slug: str) -> str:
        lowered = slug.lower()
        normalized = []
        last_dash = False
        for ch in lowered:
            if ch.isalnum():
                normalized.append(ch)
                last_dash = False
            else:
                if not last_dash:
                    normalized.append("-")
                last_dash = True
        result = "".join(normalized).strip("-")
        return result

    def build_cache_key(self, domain: Sequence[str], slug: Optional[str], query: Optional[str], limit: int) -> str:
        domain_key = ":".join(sorted(domain)) if domain else "*"
        slug_key = slug or ""
        query_hash = hash_query(query)
        return f"rules:{domain_key}:{slug_key}:{query_hash}:{limit}"

    def query(
        self,
        domain: Sequence[str],
        slug: Optional[str],
        query: Optional[str],
        limit: int,
        include_raw: bool,
        ref: Optional[str] = None,
    ) -> Tuple[Dict[str, Any], int]:
        started_at = time.monotonic()
        request_id = str(uuid.uuid4())
        cached = False
        warnings: List[WarningMessage] = []

        if ref:
            result = self._fetch_by_ref(ref, include_raw)
            http_status = 200 if result else 404
            status = "ok" if result else "not_found"
            data = [result] if result else None
            took_ms = int((time.monotonic() - started_at) * 1000)
            return self.build_response(
                status=status,
                data=data,
                warnings=warnings,
                request_id=request_id,
                took_ms=took_ms,
                cached=cached,
                http_status=http_status,
            )

        normalized_slug = self.normalize_slug(slug) if slug else None

        cache_key = self.build_cache_key(domain, normalized_slug, query, limit)
        cache_entry = self._cache.get(cache_key)
        result_payload: Optional[Dict[str, Any]] = None
        http_status = 200
        if cache_entry:
            cached = True
            cached_payload = cache_entry.value
            payload = cached_payload["payload"]
            http_status = cached_payload["http_status"]
            if time.monotonic() > cache_entry.expires_at:
                warnings.append(WarningMessage(code="CACHE_STALE", message="Resultado servido enquanto revalidação ocorre."))
            took_ms = int((time.monotonic() - started_at) * 1000)
            payload["meta"]["request_id"] = request_id
            payload["meta"]["took_ms"] = took_ms
            payload["meta"]["cached"] = True
            payload["warnings"] = payload.get("warnings") or []
            payload["warnings"].extend(self.serialize_warnings(warnings))
            return payload, http_status

        try:
            result_payload, http_status = self._execute_lookup(
                domain=domain,
                slug=normalized_slug,
                query=query,
                limit=limit,
                include_raw=include_raw,
                warnings=warnings,
            )
        except Exception as exc:  # pragma: no cover - defensive
            warnings.append(WarningMessage(code="UPSTREAM_FAILURE", message=str(exc)))
            status_payload = self.build_response(
                status="error",
                data=None,
                warnings=warnings,
                request_id=request_id,
                took_ms=int((time.monotonic() - started_at) * 1000),
                cached=False,
                http_status=502,
            )
            return status_payload

        result_payload["meta"]["request_id"] = request_id
        result_payload["meta"]["took_ms"] = int((time.monotonic() - started_at) * 1000)
        result_payload["meta"]["cached"] = False

        self._cache.set(
            cache_key,
            {"payload": result_payload, "http_status": http_status},
            CACHE_TTL_SECONDS,
            CACHE_STALE_SECONDS,
        )
        return result_payload, http_status

    def _execute_lookup(
        self,
        domain: Sequence[str],
        slug: Optional[str],
        query: Optional[str],
        limit: int,
        include_raw: bool,
        warnings: List[WarningMessage],
    ) -> Tuple[Dict[str, Any], int]:
        domains = list(domain)
        if slug:
            result = self._lookup_by_slug(domains, slug, include_raw)
            if result:
                response = self.build_response("ok", [result], warnings)
                return response, 200
            prefix_matches = self._lookup_slug_prefix(domains, slug, limit)
            if len(prefix_matches) == 1:
                response = self.build_response("ok", [self._format_rule(prefix_matches[0], include_raw)])
                return response, 200
            if prefix_matches:
                candidates = [self._candidate_payload(rule) for rule in prefix_matches[:limit]]
                response = self.build_response("ambiguous", {"candidates": candidates}, warnings)
                return response, 200
            if not query:
                response = self.build_response("not_found", None, warnings)
                return response, 404

        if query:
            results = self._lookup_text(domains, query, limit)
            if results:
                warnings.append(
                    WarningMessage(code="PARTIAL_MATCH", message="Consulta textual retornou resultados aproximados.")
                )
                formatted = [self._format_rule(rule, include_raw) for rule in results]
                response = self.build_response("ok", formatted, warnings)
                return response, 200
            response = self.build_response("not_found", None, warnings)
            return response, 404

        response = self.build_response("not_found", None, warnings)
        return response, 404

    def _lookup_by_slug(self, domains: Sequence[str], slug: str, include_raw: bool) -> Optional[Dict[str, Any]]:
        for domain in domains:
            rule = self._srd_client.fetch_by_slug(domain, slug)
            if rule:
                return self._format_rule(rule, include_raw)
        return None

    def _lookup_slug_prefix(self, domains: Sequence[str], slug: str, limit: int) -> List[SrdRule]:
        results: List[SrdRule] = []
        for domain in domains:
            results.extend(self._srd_client.search_by_slug_prefix([domain], slug, limit))
            if len(results) >= limit:
                break
        return results

    def _lookup_text(self, domains: Sequence[str], query: str, limit: int) -> List[SrdRule]:
        return self._srd_client.search_by_query(domains, query, limit)

    def _fetch_by_ref(self, ref: str, include_raw: bool) -> Optional[Dict[str, Any]]:
        rule = self._srd_client.fetch_by_ref(ref)
        if not rule:
            return None
        return self._format_rule(rule, include_raw)

    def _format_rule(self, rule: SrdRule, include_raw: bool) -> Dict[str, Any]:
        description = {"excerpt": self._build_excerpt(rule.desc)}
        if include_raw:
            description["raw"] = rule.desc
        references = None
        if rule.references:
            references = []
            for ref in rule.references:
                entry = dict(ref)
                origin = entry.get("origin", "srd")
                if origin not in {"srd", "non-srd"}:
                    origin = "non-srd"
                entry["origin"] = origin
                references.append(entry)
        return {
            "slug": rule.slug,
            "name": rule.name,
            "domain": rule.domain,
            "ref": rule.ref,
            "url": rule.url,
            "source": rule.source,
            "description": description,
            "references": references,
            "last_synced_at": rule.last_synced_at,
        }

    def _build_excerpt(self, desc: Sequence[str]) -> str:
        if not desc:
            return ""
        excerpt_paragraphs = list(desc[:2])
        return "\n\n".join(excerpt_paragraphs)

    def _candidate_payload(self, rule: SrdRule) -> Dict[str, Any]:
        return {
            "slug": rule.slug,
            "name": rule.name,
            "domain": rule.domain,
            "ref": rule.ref,
        }

    def build_response(
        self,
        status: str,
        data: Optional[Any],
        warnings: Sequence[WarningMessage] = (),
        request_id: Optional[str] = None,
        took_ms: Optional[int] = None,
        cached: Optional[bool] = None,
        http_status: Optional[int] = None,
    ) -> Tuple[Dict[str, Any], int]:
        payload: Dict[str, Any] = {
            "status": status,
            "meta": {
                "request_id": request_id or str(uuid.uuid4()),
                "took_ms": took_ms or 0,
                "cached": cached if cached is not None else False,
                "source": "srd",
            },
        }
        if data is not None:
            payload["data"] = data
        serialized_warnings = self.serialize_warnings(warnings)
        if serialized_warnings:
            payload["warnings"] = serialized_warnings
        status_code = http_status or 200
        return payload, status_code

    def serialize_warnings(self, warnings: Sequence[WarningMessage]) -> List[Dict[str, Any]]:
        serialized: List[Dict[str, Any]] = []
        for warning in warnings:
            item = {"code": warning.code, "message": warning.message}
            if warning.details:
                item["details"] = warning.details
            serialized.append(item)
        return serialized


def allowed_domains(domain_param: Optional[str]) -> List[str]:
    if not domain_param:
        return list(DOMAIN_BASE_PATH.keys())
    if domain_param not in DOMAIN_BASE_PATH:
        raise ValueError("Invalid domain")
    return [domain_param]
