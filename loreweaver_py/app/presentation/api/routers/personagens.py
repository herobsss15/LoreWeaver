"""API routes for managing personagens."""

from __future__ import annotations

from fastapi import APIRouter, Depends, HTTPException, status

from app.application.services.personagem_service import PersonagemService
from app.presentation.api.dependencies import get_personagem_service
from app.presentation.api.schemas.personagem import (
    PersonagemCreate,
    PersonagemRead,
    PersonagemUpdate,
)

router = APIRouter(prefix="/personagens", tags=["personagens"])


@router.get("/mundo/{mundo_id}", response_model=list[PersonagemRead])
def listar_personagens_por_mundo(
    mundo_id: int,
    service: PersonagemService = Depends(get_personagem_service),
) -> list[PersonagemRead]:
    personagens = service.listar_por_mundo(mundo_id)
    return [PersonagemRead.model_validate(personagem) for personagem in personagens]


@router.get("/{personagem_id}", response_model=PersonagemRead)
def obter_personagem(
    personagem_id: int,
    service: PersonagemService = Depends(get_personagem_service),
) -> PersonagemRead:
    personagem = service.obter_personagem(personagem_id)
    if personagem is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Personagem não encontrado")
    return PersonagemRead.model_validate(personagem)


@router.post("/", response_model=PersonagemRead, status_code=status.HTTP_201_CREATED)
def criar_personagem(
    payload: PersonagemCreate,
    service: PersonagemService = Depends(get_personagem_service),
) -> PersonagemRead:
    personagem = service.criar_personagem(
        nome=payload.nome,
        descricao=payload.descricao,
        papel=payload.papel,
        mundo_id=payload.mundo_id,
    )
    return PersonagemRead.model_validate(personagem)


@router.put("/{personagem_id}", response_model=PersonagemRead)
def atualizar_personagem(
    personagem_id: int,
    payload: PersonagemUpdate,
    service: PersonagemService = Depends(get_personagem_service),
) -> PersonagemRead:
    personagem = service.atualizar_personagem(
        personagem_id=personagem_id,
        nome=payload.nome,
        descricao=payload.descricao,
        papel=payload.papel,
        mundo_id=payload.mundo_id,
        ativo=payload.ativo,
    )
    return PersonagemRead.model_validate(personagem)


@router.delete("/{personagem_id}", status_code=status.HTTP_204_NO_CONTENT)
def remover_personagem(
    personagem_id: int,
    service: PersonagemService = Depends(get_personagem_service),
) -> None:
    personagem = service.obter_personagem(personagem_id)
    if personagem is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Personagem não encontrado")
    service.remover_personagem(personagem_id)
