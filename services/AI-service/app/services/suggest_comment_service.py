from app.providers.factory import get_llm_provider
from app.schemas.comment import CommentSuggestRequest

async def suggest_comment_service(payload: CommentSuggestRequest):
    llm_provider = get_llm_provider()
    return await llm_provider.suggest_comment(payload)