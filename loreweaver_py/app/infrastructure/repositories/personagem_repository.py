"""SQLAlchemy implementation of :class:`PersonagemRepository`."""

from __future__ import annotations

from typing import Iterable, List

from sqlalchemy import select
from sqlalchemy.orm import Session

from app.domain.entities.personagem import Personagem
from app.domain.repositories.personagem_repository import PersonagemRepository
from app.infrastructure.db.models.personagem import PersonagemModel


class PersonagemSQLAlchemyRepository(PersonagemRepository):
    """Repository implementation backed by SQLAlchemy sessions."""

    def __init__(self, session: Session) -> None:
        self._session = session

    def listar_por_mundo(self, mundo_id: int) -> Iterable[Personagem]:
        resultados: List[PersonagemModel] = self._session.scalars(
            select(PersonagemModel).where(PersonagemModel.mundo_id == mundo_id)
        ).all()
        return [self._to_domain(modelo) for modelo in resultados]

    def obter_por_id(self, personagem_id: int) -> Personagem | None:
        modelo = self._session.get(PersonagemModel, personagem_id)
        return self._to_domain(modelo) if modelo else None

    def adicionar(self, personagem: Personagem) -> Personagem:
        modelo = PersonagemModel(
            nome=personagem.nome,
            descricao=personagem.descricao,
            papel=personagem.papel,
            mundo_id=personagem.mundo_id,
            ativo=personagem.ativo,
        )
        self._session.add(modelo)
        self._session.commit()
        self._session.refresh(modelo)
        return self._to_domain(modelo)

    def atualizar(self, personagem: Personagem) -> Personagem:
        modelo = self._session.get(PersonagemModel, personagem.id)
        if modelo is None:
            raise ValueError("Personagem não encontrado para atualização.")
        modelo.nome = personagem.nome
        modelo.descricao = personagem.descricao
        modelo.papel = personagem.papel
        modelo.mundo_id = personagem.mundo_id
        modelo.ativo = personagem.ativo
        self._session.commit()
        self._session.refresh(modelo)
        return self._to_domain(modelo)

    def remover(self, personagem_id: int) -> None:
        modelo = self._session.get(PersonagemModel, personagem_id)
        if modelo is None:
            return
        self._session.delete(modelo)
        self._session.commit()

    @staticmethod
    def _to_domain(modelo: PersonagemModel) -> Personagem:
        return Personagem(
            id=modelo.id,
            nome=modelo.nome,
            descricao=modelo.descricao,
            papel=modelo.papel,
            mundo_id=modelo.mundo_id,
            ativo=modelo.ativo,
        )
