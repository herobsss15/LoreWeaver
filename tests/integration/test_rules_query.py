from __future__ import annotations

from typing import Any, Dict, Iterable, List, Optional

import pytest
from fastapi import FastAPI
from fastapi.testclient import TestClient

from src.app.application.services.rules_service import RulesService
from src.app.infrastructure.cache.memory_cache import MemoryCache
from src.app.infrastructure.repositories.srd_client import DOMAIN_BASE_PATH, SrdRule
from src.app.presentation.api.routers.rules import get_service, router


class FakeSrdClient:
    def __init__(self, fixtures: Dict[str, Dict[str, Any]]) -> None:
        self._fixtures = fixtures

    def fetch_by_ref(self, ref: str) -> Optional[SrdRule]:
        return self._copy_rule(self._fixtures.get(ref))

    def fetch_by_slug(self, domain: str, slug: str) -> Optional[SrdRule]:
        ref = f"{DOMAIN_BASE_PATH[domain]}/{slug}"
        return self.fetch_by_ref(ref)

    def search_by_slug_prefix(self, domains: Iterable[str], prefix: str, limit: int) -> List[SrdRule]:
        matches: List[SrdRule] = []
        for domain in domains:
            base = DOMAIN_BASE_PATH[domain]
            for ref, payload in self._fixtures.items():
                if not ref.startswith(base):
                    continue
                slug = payload["slug"]
                if slug.startswith(prefix):
                    matches.append(self._copy_rule(payload))
                if len(matches) >= limit:
                    return matches
        return matches

    def search_by_query(self, domains: Iterable[str], query: str, limit: int) -> List[SrdRule]:
        q_lower = query.lower()
        matches: List[tuple[int, Dict[str, Any]]] = []
        for domain in domains:
            base = DOMAIN_BASE_PATH[domain]
            for ref, payload in self._fixtures.items():
                if not ref.startswith(base):
                    continue
                haystack = f"{payload['name']} {' '.join(payload['desc'])}".lower()
                if q_lower in haystack:
                    matches.append((haystack.index(q_lower), payload))
        matches.sort(key=lambda item: item[0])
        return [self._copy_rule(payload) for _, payload in matches[:limit]]

    def _copy_rule(self, payload: Optional[Dict[str, Any]]) -> Optional[SrdRule]:
        if not payload:
            return None
        return SrdRule(
            slug=payload["slug"],
            name=payload["name"],
            domain=payload["domain"],
            ref=payload["ref"],
            url=payload["url"],
            source="5e-srd",
            desc=list(payload["desc"]),
            references=payload.get("references"),
            last_synced_at=payload.get("last_synced_at"),
        )


@pytest.fixture()
def test_client() -> TestClient:
    fixtures = {
        "/api/rules/ability-checks": {
            "slug": "ability-checks",
            "name": "Ability Checks",
            "domain": "rules",
            "ref": "/api/rules/ability-checks",
            "url": "https://www.dnd5eapi.co/api/rules/ability-checks",
            "desc": [
                "When you attempt a task that calls for an ability check...",
                "The DM might call for...",
            ],
            "references": [
                {"title": "Ability Checks", "origin": "srd"}
            ],
            "last_synced_at": "2025-02-10T18:30:22Z",
        },
        "/api/rules/ability-scores": {
            "slug": "ability-scores",
            "name": "Ability Scores",
            "domain": "rules",
            "ref": "/api/rules/ability-scores",
            "url": "https://www.dnd5eapi.co/api/rules/ability-scores",
            "desc": [
                "Six abilities provide a quick description...",
                "The three main rolls are...",
            ],
            "last_synced_at": "2025-02-10T18:30:22Z",
        },
        "/api/rule-sections/conditions": {
            "slug": "conditions",
            "name": "Conditions",
            "domain": "rule-sections",
            "ref": "/api/rule-sections/conditions",
            "url": "https://www.dnd5eapi.co/api/rule-sections/conditions",
            "desc": [
                "Conditions alter your capabilities in a variety of ways...",
                "A condition lasts until...",
            ],
            "last_synced_at": "2025-02-10T18:30:22Z",
        },
    }
    fake_client = FakeSrdClient(fixtures)
    service = RulesService(fake_client, MemoryCache())

    app = FastAPI()
    app.include_router(router)

    app.dependency_overrides[get_service] = lambda: service
    return TestClient(app)


def test_query_by_ref(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"ref": "/api/rules/ability-checks"})
    body = response.json()
    assert response.status_code == 200
    assert body["status"] == "ok"
    assert body["data"][0]["ref"] == "/api/rules/ability-checks"
    assert body["meta"]["cached"] is False


def test_query_by_slug(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"slug": "Ability-Checks"})
    body = response.json()
    assert response.status_code == 200
    assert body["status"] == "ok"
    assert body["data"][0]["slug"] == "ability-checks"
    assert body["data"][0]["ref"] == "/api/rules/ability-checks"


def test_query_text_partial_match(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"q": "ability", "limit": 3})
    body = response.json()
    assert response.status_code == 200
    assert body["status"] == "ok"
    assert any(warning["code"] == "PARTIAL_MATCH" for warning in body.get("warnings", []))
    assert len(body["data"]) <= 3


def test_query_not_found(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"slug": "inexistente"})
    body = response.json()
    assert response.status_code == 404
    assert body["status"] == "not_found"
    assert "data" not in body


def test_query_ambiguous(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"slug": "abil"})
    body = response.json()
    assert response.status_code == 200
    assert body["status"] == "ambiguous"
    assert "candidates" in body.get("data", {})
    assert len(body["data"]["candidates"]) >= 2


def test_cache_hits_mark_cached(test_client: TestClient) -> None:
    params = {"slug": "ability-checks"}
    first = test_client.get("/rules/query", params=params)
    assert first.status_code == 200
    second = test_client.get("/rules/query", params=params)
    body = second.json()
    assert second.status_code == 200
    assert body["meta"]["cached"] is True


def test_invalid_limit_returns_refuse(test_client: TestClient) -> None:
    response = test_client.get("/rules/query", params={"limit": 50})
    body = response.json()
    assert response.status_code == 422
    assert body["status"] == "refuse"
    assert any(w["code"] == "INVALID_PARAMS" for w in body.get("warnings", []))
