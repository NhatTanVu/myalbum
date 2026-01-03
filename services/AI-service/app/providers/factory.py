from app.config import settings
from app.providers.LLM.base import LLMProvider
from app.providers.LLM.openai_provider import OpenAIProvider

_LLM_PROVIDER_INSTANCE: LLMProvider | None = None


def get_llm_provider() -> LLMProvider:
    global _LLM_PROVIDER_INSTANCE

    if _LLM_PROVIDER_INSTANCE is not None:
        return _LLM_PROVIDER_INSTANCE

    if settings.LLM_PROVIDER == "openai":
        _LLM_PROVIDER_INSTANCE = OpenAIProvider()
        return _LLM_PROVIDER_INSTANCE

    raise ValueError("Unsupported LLM Provider")
