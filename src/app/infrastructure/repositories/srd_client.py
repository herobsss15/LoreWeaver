from __future__ import annotations

import hashlib
from dataclasses import dataclass
from typing import Any, Dict, Iterable, List, Optional

import httpx

DOMAIN_BASE_PATH = {
    "rules": "/api/rules",
    "rule-sections": "/api/rule-sections",
}


@dataclass
class SrdRule:
    slug: str
    name: str
    domain: str
    ref: str
    url: str
    source: str
    desc: List[str]
    references: Optional[List[Dict[str, Any]]]
    last_synced_at: Optional[str]


class SrdClient:
    """Minimal client for 5e-SRD API interactions."""

    def __init__(self, base_url: str, http_client: Optional[httpx.Client] = None) -> None:
        self._base_url = base_url.rstrip("/")
        self._http_client = http_client or httpx.Client(base_url=self._base_url, timeout=10.0)

    def fetch_by_ref(self, ref: str) -> Optional[SrdRule]:
        response = self._http_client.get(ref)
        if response.status_code == 404:
            return None
        response.raise_for_status()
        return self._parse_rule(response.json(), ref)

    def fetch_by_slug(self, domain: str, slug: str) -> Optional[SrdRule]:
        path = f"{DOMAIN_BASE_PATH[domain]}/{slug}"
        response = self._http_client.get(path)
        if response.status_code == 404:
            return None
        response.raise_for_status()
        return self._parse_rule(response.json(), path)

    def search_by_slug_prefix(self, domains: Iterable[str], prefix: str, limit: int) -> List[SrdRule]:
        results: List[SrdRule] = []
        for domain in domains:
            collection = self._fetch_collection(domain)
            for item in collection:
                slug = item.get("index", "")
                if slug.startswith(prefix):
                    rule = self.fetch_by_slug(domain, slug)
                    if rule:
                        results.append(rule)
                if len(results) >= limit:
                    return results
        return results

    def search_by_query(self, domains: Iterable[str], query: str, limit: int) -> List[SrdRule]:
        normalized = query.lower()
        results: List[SrdRule] = []
        for domain in domains:
            collection = self._fetch_collection(domain)
            ranked: List[tuple[int, Dict[str, Any]]] = []
            for item in collection:
                name = item.get("name", "")
                desc = " ".join(item.get("desc", []))
                haystack = f"{name} {desc}".lower()
                if normalized in haystack:
                    score = -haystack.index(normalized)
                    ranked.append((score, item))
            for _, item in sorted(ranked)[:limit]:
                rule = self.fetch_by_slug(domain, item["index"])
                if rule:
                    results.append(rule)
                if len(results) >= limit:
                    return results
        return results

    def _fetch_collection(self, domain: str) -> List[Dict[str, Any]]:
        path = DOMAIN_BASE_PATH[domain]
        response = self._http_client.get(path)
        response.raise_for_status()
        payload = response.json()
        return payload.get("results", [])

    def _parse_rule(self, payload: Dict[str, Any], ref: str) -> SrdRule:
        desc = payload.get("desc") or []
        slug = payload.get("index") or self._infer_slug(ref)
        references = payload.get("references")
        return SrdRule(
            slug=slug,
            name=payload.get("name", slug.replace("-", " ").title()),
            domain=self._infer_domain(ref),
            ref=ref,
            url=f"{self._base_url}{ref}",
            source="5e-srd",
            desc=desc,
            references=references,
            last_synced_at=None,
        )

    def _infer_domain(self, ref: str) -> str:
        for domain, base in DOMAIN_BASE_PATH.items():
            if ref.startswith(base):
                return domain
        raise ValueError(f"Unknown ref domain: {ref}")

    def _infer_slug(self, ref: str) -> str:
        return ref.rstrip("/").split("/")[-1]


def hash_query(value: Optional[str]) -> str:
    if not value:
        return ""
    return hashlib.sha1(value.encode("utf-8")).hexdigest()
