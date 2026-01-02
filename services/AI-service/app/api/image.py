from fastapi import APIRouter, HTTPException
from app.schemas.image import ImageDescribeRequest, ImageDescribeResponse
from app.services.describe_image_service import describe_image_service

router = APIRouter()


@router.post("/describe", response_model=ImageDescribeResponse)
async def describe_image(payload: ImageDescribeRequest):
    try:
        result = await describe_image_service(payload)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
