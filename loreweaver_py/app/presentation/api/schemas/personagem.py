"""Pydantic schemas for character endpoints."""

from __future__ import annotations

from pydantic import BaseModel, ConfigDict, Field


class PersonagemBase(BaseModel):
    nome: str = Field(..., min_length=1, max_length=255)
    descricao: str = Field(..., min_length=1, max_length=1024)
    papel: str = Field(..., min_length=1, max_length=255)
    mundo_id: int = Field(..., gt=0)


class PersonagemCreate(PersonagemBase):
    pass


class PersonagemUpdate(PersonagemBase):
    ativo: bool = True


class PersonagemRead(PersonagemBase):
    model_config = ConfigDict(from_attributes=True)

    id: int
    ativo: bool
