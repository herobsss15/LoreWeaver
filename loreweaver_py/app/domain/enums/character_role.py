"""Available roles for characters."""

from enum import Enum


class CharacterRole(str, Enum):
    protagonista = "Protagonista"
    antagonista = "Antagonista"
    secundario = "Secundario"
    npc = "NPC"
