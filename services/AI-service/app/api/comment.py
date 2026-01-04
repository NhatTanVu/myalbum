from fastapi import APIRouter, Depends
from app.schemas.comment import CommentSuggestRequest, CommentSuggestResponse
from app.services.suggest_comment_service import suggest_comment_service
from fastapi import HTTPException
from app.security.jwt import get_current_user

router = APIRouter()


@router.post("/suggest", response_model=CommentSuggestResponse)
async def suggest(
        payload: CommentSuggestRequest,
        user=Depends(get_current_user)):
    try:
        result = await suggest_comment_service(payload)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
