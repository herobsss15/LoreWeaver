from __future__ import annotations

from fastapi import APIRouter, Depends, Query, status
from fastapi.responses import JSONResponse

from src.app.application.services.rules_service import RulesService, WarningMessage, allowed_domains
from src.app.infrastructure.cache.memory_cache import MemoryCache
from src.app.infrastructure.repositories.srd_client import DOMAIN_BASE_PATH, SrdClient

router = APIRouter()

_CACHE = MemoryCache()
_CLIENT = SrdClient(base_url="https://www.dnd5eapi.co")
_SERVICE = RulesService(srd_client=_CLIENT, cache=_CACHE)


def get_service() -> RulesService:
    return _SERVICE


@router.get("/rules/query")
def query_rules(
    domain: str | None = Query(default=None),
    slug: str | None = Query(default=None, min_length=1, max_length=80),
    q: str | None = Query(default=None, min_length=1, max_length=100),
    limit: int = Query(default=5, ge=1),
    include_raw: bool = Query(default=False),
    ref: str | None = Query(default=None, min_length=1, max_length=160),
    service: RulesService = Depends(get_service),
):
    if ref and not any(ref.startswith(path) for path in DOMAIN_BASE_PATH.values()):
        payload, status_code = service.build_response(
            status="refuse",
            data=None,
            warnings=[WarningMessage(code="INVALID_PARAMS", message="ref inválido para domínio rules.")],
            http_status=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )
        return JSONResponse(status_code=status_code, content=payload)

    try:
        domains = allowed_domains(domain)
    except ValueError:
        payload, status_code = service.build_response(
            status="refuse",
            data=None,
            warnings=[WarningMessage(code="INVALID_PARAMS", message="domain deve ser rules ou rule-sections.")],
            http_status=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )
        return JSONResponse(status_code=status_code, content=payload)

    if not any([slug, q, ref]):
        payload, status_code = service.build_response(
            status="refuse",
            data=None,
            warnings=[WarningMessage(code="INVALID_PARAMS", message="Informe slug, q ou ref.")],
            http_status=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )
        return JSONResponse(status_code=status_code, content=payload)

    if limit > 10:
        payload, status_code = service.build_response(
            status="refuse",
            data=None,
            warnings=[WarningMessage(code="INVALID_PARAMS", message="limit deve ser menor ou igual a 10.")],
            http_status=status.HTTP_422_UNPROCESSABLE_ENTITY,
        )
        return JSONResponse(status_code=status_code, content=payload)

    payload, status_code = service.query(
        domain=domains,
        slug=slug,
        query=q,
        limit=limit,
        include_raw=include_raw,
        ref=ref,
    )
    return JSONResponse(status_code=status_code, content=payload)
