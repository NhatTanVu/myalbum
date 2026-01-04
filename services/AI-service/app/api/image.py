from fastapi import APIRouter, HTTPException, Depends
from app.schemas.image import ImageDescribeRequest, ImageDescribeResponse
from app.services.describe_image_service import describe_image_service
from app.security.jwt import get_current_user

router = APIRouter()


@router.post("/describe", response_model=ImageDescribeResponse)
async def describe_image(
        payload: ImageDescribeRequest,
        user=Depends(get_current_user)):
    try:
        result = await describe_image_service(payload)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
