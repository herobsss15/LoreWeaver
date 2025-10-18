"""API routes for managing mundos."""

from __future__ import annotations

from fastapi import APIRouter, Depends, HTTPException, status

from app.application.services.mundo_service import MundoService
from app.presentation.api.dependencies import get_mundo_service
from app.presentation.api.schemas.mundo import MundoCreate, MundoRead, MundoUpdate

router = APIRouter(prefix="/mundos", tags=["mundos"])


@router.get("/", response_model=list[MundoRead])
def listar_mundos(service: MundoService = Depends(get_mundo_service)) -> list[MundoRead]:
    return [MundoRead.model_validate(mundo) for mundo in service.listar_mundos()]


@router.get("/{mundo_id}", response_model=MundoRead)
def obter_mundo(
    mundo_id: int,
    service: MundoService = Depends(get_mundo_service),
) -> MundoRead:
    mundo = service.obter_mundo(mundo_id)
    if mundo is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Mundo não encontrado")
    return MundoRead.model_validate(mundo)


@router.post("/", response_model=MundoRead, status_code=status.HTTP_201_CREATED)
def criar_mundo(
    payload: MundoCreate,
    service: MundoService = Depends(get_mundo_service),
) -> MundoRead:
    mundo = service.criar_mundo(nome=payload.nome, descricao=payload.descricao)
    return MundoRead.model_validate(mundo)


@router.put("/{mundo_id}", response_model=MundoRead)
def atualizar_mundo(
    mundo_id: int,
    payload: MundoUpdate,
    service: MundoService = Depends(get_mundo_service),
) -> MundoRead:
    mundo = service.atualizar_mundo(
        mundo_id=mundo_id,
        nome=payload.nome,
        descricao=payload.descricao,
        ativo=payload.ativo,
    )
    return MundoRead.model_validate(mundo)


@router.delete("/{mundo_id}", status_code=status.HTTP_204_NO_CONTENT)
def remover_mundo(
    mundo_id: int,
    service: MundoService = Depends(get_mundo_service),
) -> None:
    mundo = service.obter_mundo(mundo_id)
    if mundo is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Mundo não encontrado")
    service.remover_mundo(mundo_id)
