"""Application service for character-related use cases."""

from __future__ import annotations

from typing import Iterable

from app.domain.entities.personagem import Personagem
from app.domain.repositories.personagem_repository import PersonagemRepository


class PersonagemService:
    """Use cases for managing characters."""

    def __init__(self, repository: PersonagemRepository) -> None:
        self._repository = repository

    def listar_por_mundo(self, mundo_id: int) -> Iterable[Personagem]:
        return self._repository.listar_por_mundo(mundo_id)

    def obter_personagem(self, personagem_id: int) -> Personagem | None:
        return self._repository.obter_por_id(personagem_id)

    def criar_personagem(
        self,
        nome: str,
        descricao: str,
        papel: str,
        mundo_id: int,
    ) -> Personagem:
        personagem = Personagem(
            nome=nome,
            descricao=descricao,
            papel=papel,
            mundo_id=mundo_id,
        )
        return self._repository.adicionar(personagem)

    def atualizar_personagem(
        self,
        personagem_id: int,
        nome: str,
        descricao: str,
        papel: str,
        mundo_id: int,
        ativo: bool,
    ) -> Personagem:
        personagem = Personagem(
            nome=nome,
            descricao=descricao,
            papel=papel,
            mundo_id=mundo_id,
            ativo=ativo,
            id=personagem_id,
        )
        return self._repository.atualizar(personagem)

    def remover_personagem(self, personagem_id: int) -> None:
        self._repository.remover(personagem_id)
