from __future__ import annotations

import threading
import time
from dataclasses import dataclass
from typing import Any, Dict, Optional


@dataclass
class CacheEntry:
    value: Any
    expires_at: float
    stale_at: float


class MemoryCache:
    """In-memory cache supporting TTL and stale-while-revalidate."""

    def __init__(self) -> None:
        self._store: Dict[str, CacheEntry] = {}
        self._lock = threading.Lock()

    def get(self, key: str) -> Optional[CacheEntry]:
        with self._lock:
            entry = self._store.get(key)
            if entry is None:
                return None
            now = time.monotonic()
            if now > entry.stale_at:
                # Entry completely expired; drop it.
                self._store.pop(key, None)
                return None
            return entry

    def set(self, key: str, value: Any, ttl_seconds: int, stale_seconds: int) -> None:
        expires_at = time.monotonic() + ttl_seconds
        stale_at = expires_at + stale_seconds
        with self._lock:
            self._store[key] = CacheEntry(value=value, expires_at=expires_at, stale_at=stale_at)

    def clear(self) -> None:
        with self._lock:
            self._store.clear()
