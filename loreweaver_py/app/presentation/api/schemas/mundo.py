"""Pydantic schemas for world endpoints."""

from __future__ import annotations

from pydantic import BaseModel, ConfigDict, Field


class MundoBase(BaseModel):
    nome: str = Field(..., min_length=1, max_length=255)
    descricao: str = Field(..., min_length=1, max_length=1024)


class MundoCreate(MundoBase):
    pass


class MundoUpdate(MundoBase):
    ativo: bool = True


class MundoRead(MundoBase):
    model_config = ConfigDict(from_attributes=True)

    id: int
    ativo: bool
