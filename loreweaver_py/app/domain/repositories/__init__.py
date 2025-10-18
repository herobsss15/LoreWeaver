"""Domain repository exports."""

from app.domain.repositories.mundo_repository import MundoRepository
from app.domain.repositories.personagem_repository import PersonagemRepository

__all__ = ["MundoRepository", "PersonagemRepository"]
