"""FastAPI entrypoint for the LoreWeaver Python service."""

from __future__ import annotations

from fastapi import FastAPI

from app.core.config import settings
from app.infrastructure.db.base import Base
from app.infrastructure.db.session import engine
from app.presentation.api.routers import mundos, personagens

# Create tables if they do not exist. In production prefer Alembic migrations.
Base.metadata.create_all(bind=engine)

app = FastAPI(title=settings.app_name, debug=settings.debug)

app.include_router(mundos.router)
app.include_router(personagens.router)


@app.get("/health", tags=["health"])
def healthcheck() -> dict[str, str]:
    """Simple healthcheck endpoint."""

    return {"status": "ok"}
