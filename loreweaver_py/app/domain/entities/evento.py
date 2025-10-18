"""Domain entity representing an event."""

from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime


@dataclass(slots=True)
class Evento:
    """An event associated with a world."""

    nome: str
    descricao: str
    data: datetime
    mundo_id: int
    criador_id: int
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.nome:
            raise ValueError("O nome do evento não pode ser vazio.")
        if not self.descricao:
            raise ValueError("A descrição do evento não pode ser vazia.")
        if self.data == datetime.min:
            raise ValueError("A data do evento é inválida.")
        if self.mundo_id <= 0:
            raise ValueError("O identificador do mundo deve ser positivo.")
        if self.criador_id <= 0:
            raise ValueError("O identificador do criador deve ser positivo.")

    def deletar(self) -> None:
        self.ativo = False

    def restaurar(self) -> None:
        self.ativo = True
