"""Repository contract for :class:`Mundo`."""

from __future__ import annotations

from abc import ABC, abstractmethod
from typing import Iterable

from app.domain.entities.mundo import Mundo


class MundoRepository(ABC):
    """Abstract base repository for :class:`Mundo`."""

    @abstractmethod
    def listar(self) -> Iterable[Mundo]:
        """Return all registered worlds."""

    @abstractmethod
    def obter_por_id(self, mundo_id: int) -> Mundo | None:
        """Return a world by its identifier."""

    @abstractmethod
    def adicionar(self, mundo: Mundo) -> Mundo:
        """Persist a new world and return the stored entity."""

    @abstractmethod
    def atualizar(self, mundo: Mundo) -> Mundo:
        """Update an existing world."""

    @abstractmethod
    def remover(self, mundo_id: int) -> None:
        """Remove a world by its identifier."""
