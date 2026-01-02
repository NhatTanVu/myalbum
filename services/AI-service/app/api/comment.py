from fastapi import APIRouter
from app.schemas.comment import CommentSuggestRequest, CommentSuggestResponse
from app.services.suggest_comment_service import suggest_comment_service
from fastapi import HTTPException

router = APIRouter()


@router.post("/suggest", response_model=CommentSuggestResponse)
async def suggest(payload: CommentSuggestRequest):
    try:
        result = await suggest_comment_service(payload)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
