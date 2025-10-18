"""Domain entity representing a version (versão) of a world."""

from __future__ import annotations

from dataclasses import dataclass


@dataclass(slots=True)
class Versao:
    """Version history entry for a world."""

    numero: str
    descricao_mudancas: str
    mundo_id: int
    criador_id: int
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.numero:
            raise ValueError("O número da versão não pode ser vazio.")
        if len(self.descricao_mudancas) < 30:
            raise ValueError("A descrição das mudanças deve ter pelo menos 30 caracteres.")
        if self.mundo_id <= 0:
            raise ValueError("O identificador do mundo deve ser positivo.")
        if self.criador_id <= 0:
            raise ValueError("O identificador do criador deve ser positivo.")

    def deletar(self) -> None:
        self.ativo = False

    def restaurar(self) -> None:
        self.ativo = True
