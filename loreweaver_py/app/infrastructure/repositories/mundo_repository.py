"""SQLAlchemy implementation of :class:`MundoRepository`."""

from __future__ import annotations

from typing import Iterable, List

from sqlalchemy import select
from sqlalchemy.orm import Session

from app.domain.entities.mundo import Mundo
from app.domain.repositories.mundo_repository import MundoRepository
from app.infrastructure.db.models.mundo import MundoModel


class MundoSQLAlchemyRepository(MundoRepository):
    """Repository implementation backed by SQLAlchemy sessions."""

    def __init__(self, session: Session) -> None:
        self._session = session

    def listar(self) -> Iterable[Mundo]:
        resultados: List[MundoModel] = self._session.scalars(select(MundoModel)).all()
        return [self._to_domain(modelo) for modelo in resultados]

    def obter_por_id(self, mundo_id: int) -> Mundo | None:
        modelo = self._session.get(MundoModel, mundo_id)
        return self._to_domain(modelo) if modelo else None

    def adicionar(self, mundo: Mundo) -> Mundo:
        modelo = MundoModel(nome=mundo.nome, descricao=mundo.descricao, ativo=mundo.ativo)
        self._session.add(modelo)
        self._session.commit()
        self._session.refresh(modelo)
        return self._to_domain(modelo)

    def atualizar(self, mundo: Mundo) -> Mundo:
        modelo = self._session.get(MundoModel, mundo.id)
        if modelo is None:
            raise ValueError("Mundo não encontrado para atualização.")
        modelo.nome = mundo.nome
        modelo.descricao = mundo.descricao
        modelo.ativo = mundo.ativo
        self._session.commit()
        self._session.refresh(modelo)
        return self._to_domain(modelo)

    def remover(self, mundo_id: int) -> None:
        modelo = self._session.get(MundoModel, mundo_id)
        if modelo is None:
            return
        self._session.delete(modelo)
        self._session.commit()

    @staticmethod
    def _to_domain(modelo: MundoModel) -> Mundo:
        return Mundo(
            id=modelo.id,
            nome=modelo.nome,
            descricao=modelo.descricao,
            ativo=modelo.ativo,
        )
