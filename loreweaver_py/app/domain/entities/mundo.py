"""Domain entity representing a world (mundo)."""

from __future__ import annotations

from dataclasses import dataclass


@dataclass(slots=True)
class Mundo:
    """Aggregate root for a fictional world."""

    nome: str
    descricao: str
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.nome:
            raise ValueError("O nome do mundo não pode ser vazio.")
        if not self.descricao:
            raise ValueError("A descrição do mundo não pode ser vazia.")

    def deletar(self) -> None:
        """Mark the world as inactive."""

        self.ativo = False

    def restaurar(self) -> None:
        """Mark the world as active."""

        self.ativo = True
