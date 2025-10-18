"""Repository contract for :class:`Personagem`."""

from __future__ import annotations

from abc import ABC, abstractmethod
from typing import Iterable

from app.domain.entities.personagem import Personagem


class PersonagemRepository(ABC):
    """Abstract base repository for characters."""

    @abstractmethod
    def listar_por_mundo(self, mundo_id: int) -> Iterable[Personagem]:
        """Return all characters that belong to a world."""

    @abstractmethod
    def obter_por_id(self, personagem_id: int) -> Personagem | None:
        """Return a character by its identifier."""

    @abstractmethod
    def adicionar(self, personagem: Personagem) -> Personagem:
        """Persist a new character."""

    @abstractmethod
    def atualizar(self, personagem: Personagem) -> Personagem:
        """Update an existing character."""

    @abstractmethod
    def remover(self, personagem_id: int) -> None:
        """Remove a character by its identifier."""
