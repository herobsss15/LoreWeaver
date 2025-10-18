"""SQLAlchemy model for :class:`Mundo`."""

from __future__ import annotations

from typing import TYPE_CHECKING

from sqlalchemy import Boolean, Integer, String
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.infrastructure.db.base import Base

if TYPE_CHECKING:  # pragma: no cover - circular type hints only
    from app.infrastructure.db.models.personagem import PersonagemModel


class MundoModel(Base):
    """Persistence model for worlds."""

    __tablename__ = "mundos"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String(255), nullable=False)
    descricao: Mapped[str] = mapped_column(String(1024), nullable=False)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True, nullable=False)

    personagens: Mapped[list["PersonagemModel"]] = relationship(
        back_populates="mundo",
        cascade="all, delete-orphan",
    )
