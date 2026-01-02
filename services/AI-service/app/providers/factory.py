from app.config import settings
from app.providers.LLM.base import LLMProvider
from app.providers.LLM.openai_provider import OpenAIProvider


def get_llm_provider() -> LLMProvider:
    if settings.LLM_PROVIDER == "openai":
        return OpenAIProvider()
    raise ValueError("Unsupported LLM Provider")
