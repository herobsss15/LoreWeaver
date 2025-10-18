"""Domain entity representing a character."""

from __future__ import annotations

from dataclasses import dataclass


@dataclass(slots=True)
class Personagem:
    """A character that belongs to a world."""

    nome: str
    descricao: str
    papel: str
    mundo_id: int
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.nome:
            raise ValueError("O nome do personagem não pode ser vazio.")
        if not self.descricao:
            raise ValueError("A descrição do personagem não pode ser vazia.")
        if not self.papel:
            raise ValueError("O papel do personagem não pode ser vazio.")
        if self.mundo_id <= 0:
            raise ValueError("O identificador do mundo deve ser positivo.")

    def deletar(self) -> None:
        self.ativo = False

    def restaurar(self) -> None:
        self.ativo = True
