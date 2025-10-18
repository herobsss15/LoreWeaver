"""Domain entity representing a user."""

from __future__ import annotations

from dataclasses import dataclass
import re


_EMAIL_REGEX = re.compile(r"^[^@\s]+@[^@\s]+\.[^@\s]+$")


@dataclass(slots=True)
class Usuario:
    """A user who can manage worlds and related resources."""

    nome: str
    email: str
    senha: str
    ativo: bool = True
    id: int | None = None

    def __post_init__(self) -> None:
        if not self.nome:
            raise ValueError("O nome do usuário não pode ser vazio.")
        if not _EMAIL_REGEX.match(self.email):
            raise ValueError("O email do usuário é inválido.")
        if len(self.senha) < 6:
            raise ValueError("A senha do usuário deve ter pelo menos 6 caracteres.")

    def deletar(self) -> None:
        self.ativo = False

    def restaurar(self) -> None:
        self.ativo = True
