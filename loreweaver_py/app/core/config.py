"""Application configuration and settings management."""

from functools import lru_cache
from pydantic import Field
from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    """Application wide settings loaded from environment variables."""

    model_config = SettingsConfigDict(env_file=".env", extra="ignore")

    app_name: str = Field(default="LoreWeaver API")
    debug: bool = Field(default=True)
    database_url: str = Field(
        default="mysql+pymysql://loreweaver:loreweaver@localhost:3306/loreweaver",
        description="SQLAlchemy compatible database URL",
    )


@lru_cache(maxsize=1)
def get_settings() -> Settings:
    """Return cached application settings instance."""

    return Settings()


settings = get_settings()
