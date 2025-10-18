"""Categories of places."""

from enum import Enum


class TipoLugar(str, Enum):
    cidade = "Cidade"
    masmorra = "Masmorra"
    floresta = "Floresta"
    local_mistico = "Local Mistico"
