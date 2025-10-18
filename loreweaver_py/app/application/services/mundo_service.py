"""Application service orchestrating world use cases."""

from __future__ import annotations

from typing import Iterable

from app.domain.entities.mundo import Mundo
from app.domain.repositories.mundo_repository import MundoRepository


class MundoService:
    """Use cases for managing :class:`Mundo` entities."""

    def __init__(self, repository: MundoRepository) -> None:
        self._repository = repository

    def listar_mundos(self) -> Iterable[Mundo]:
        return self._repository.listar()

    def obter_mundo(self, mundo_id: int) -> Mundo | None:
        return self._repository.obter_por_id(mundo_id)

    def criar_mundo(self, nome: str, descricao: str) -> Mundo:
        mundo = Mundo(nome=nome, descricao=descricao)
        return self._repository.adicionar(mundo)

    def atualizar_mundo(
        self,
        mundo_id: int,
        nome: str,
        descricao: str,
        ativo: bool,
    ) -> Mundo:
        mundo = Mundo(nome=nome, descricao=descricao, ativo=ativo, id=mundo_id)
        return self._repository.atualizar(mundo)

    def remover_mundo(self, mundo_id: int) -> None:
        self._repository.remover(mundo_id)
