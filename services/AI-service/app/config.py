from pydantic_settings import BaseSettings, SettingsConfigDict
from pydantic import Field
from typing import List


class Settings(BaseSettings):
    # Core
    ENV: str = Field(default="development")

    CORS_ORIGINS: List[str] = Field(default_factory=list)

    # LLM
    LLM_PROVIDER: str = Field(default="openai")

    # OpenAI
    OPENAI_API_KEY: str
    OPENAI_MODEL: str = Field(default="gpt-4o-mini")
    OPENAI_TIMEOUT: int = Field(default=90)

    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
    )


settings = Settings()
