"""User profile classifications."""

from enum import Enum


class TipoUsuario(str, Enum):
    administrador = "Administrador"
    narrador = "Narrador"
    jogador = "Jogador"
