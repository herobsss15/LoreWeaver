"""Domain entity representing a place."""

from __future__ import annotations

from dataclasses import dataclass


@dataclass(slots=True)
class Lugar:
    """A significant place within a world."""

    nome: str
    descricao: str
    coordenadas: str
    mundo_id: int
    criador_id: int
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.nome:
            raise ValueError("O nome do lugar não pode ser vazio.")
        if not self.descricao:
            raise ValueError("A descrição do lugar não pode ser vazia.")
        if not self.coordenadas:
            raise ValueError("As coordenadas do lugar não podem ser vazias.")
        if self.mundo_id <= 0:
            raise ValueError("O identificador do mundo deve ser positivo.")
        if self.criador_id <= 0:
            raise ValueError("O identificador do criador deve ser positivo.")

    def deletar(self) -> None:
        self.ativo = False

    def restaurar(self) -> None:
        self.ativo = True
