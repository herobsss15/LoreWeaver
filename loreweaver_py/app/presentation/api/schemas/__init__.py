"""Pydantic schema exports."""

from app.presentation.api.schemas.mundo import MundoCreate, MundoRead, MundoUpdate
from app.presentation.api.schemas.personagem import (
    PersonagemCreate,
    PersonagemRead,
    PersonagemUpdate,
)

__all__ = [
    "MundoCreate",
    "MundoRead",
    "MundoUpdate",
    "PersonagemCreate",
    "PersonagemRead",
    "PersonagemUpdate",
]
