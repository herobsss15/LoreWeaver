"""SQLAlchemy model for :class:`Personagem`."""

from __future__ import annotations

from typing import TYPE_CHECKING

from sqlalchemy import Boolean, ForeignKey, Integer, String
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.infrastructure.db.base import Base

if TYPE_CHECKING:  # pragma: no cover - circular type hints only
    from app.infrastructure.db.models.mundo import MundoModel


class PersonagemModel(Base):
    """Persistence model for characters."""

    __tablename__ = "personagens"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String(255), nullable=False)
    descricao: Mapped[str] = mapped_column(String(1024), nullable=False)
    papel: Mapped[str] = mapped_column(String(255), nullable=False)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True, nullable=False)

    mundo_id: Mapped[int] = mapped_column(ForeignKey("mundos.id"), nullable=False)
    mundo: Mapped["MundoModel"] = relationship(back_populates="personagens")
