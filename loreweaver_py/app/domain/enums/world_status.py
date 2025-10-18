"""Possible publication states for a world."""

from enum import Enum


class WorldStatus(str, Enum):
    rascunho = "Rascunho"
    publicado = "Publicado"
    arquivado = "Arquivado"
