"""Type of events that can occur in a world."""

from enum import Enum


class TipoEvento(str, Enum):
    social = "Social"
    batalha = "Batalha"
    descoberta = "Descoberta"
    misterio = "Misterio"
