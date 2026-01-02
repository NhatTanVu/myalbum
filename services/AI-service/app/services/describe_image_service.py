from app.providers.factory import get_llm_provider
from app.schemas.image import ImageDescribeRequest

async def describe_image_service(payload: ImageDescribeRequest):
    llm_provider = get_llm_provider()
    return await llm_provider.describe_image(payload)