"""Dependency definitions for FastAPI routers."""

from __future__ import annotations

from fastapi import Depends
from sqlalchemy.orm import Session

from app.application.services.mundo_service import MundoService
from app.application.services.personagem_service import PersonagemService
from app.domain.repositories.mundo_repository import MundoRepository
from app.domain.repositories.personagem_repository import PersonagemRepository
from app.infrastructure.db.session import get_session
from app.infrastructure.repositories.mundo_repository import MundoSQLAlchemyRepository
from app.infrastructure.repositories.personagem_repository import (
    PersonagemSQLAlchemyRepository,
)


def get_db_session() -> Session:
    """Provide a new SQLAlchemy session for each request."""

    yield from get_session()


def get_mundo_repository(session: Session = Depends(get_db_session)) -> MundoRepository:
    return MundoSQLAlchemyRepository(session=session)


def get_personagem_repository(
    session: Session = Depends(get_db_session),
) -> PersonagemRepository:
    return PersonagemSQLAlchemyRepository(session=session)


def get_mundo_service(
    repository: MundoRepository = Depends(get_mundo_repository),
) -> MundoService:
    return MundoService(repository=repository)


def get_personagem_service(
    repository: PersonagemRepository = Depends(get_personagem_repository),
) -> PersonagemService:
    return PersonagemService(repository=repository)
