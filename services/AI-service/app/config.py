from pydantic_settings import BaseSettings
from pydantic import Field


class Settings(BaseSettings):
    # Core
    ENV: str = Field(default="development")

    # LLM
    LLM_PROVIDER: str = Field(default="openai")

    # OpenAI
    OPENAI_API_KEY: str
    OPENAI_MODEL: str = Field(default="gpt-4o-mini")
    OPENAI_TIMEOUT: int = Field(default=30)

    class Config:
        env_file = ".env"
        env_file_encoding = "utf-8"


settings = Settings()
