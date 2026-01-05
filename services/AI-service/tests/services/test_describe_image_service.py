import pytest
from unittest.mock import AsyncMock, patch
from app.schemas.image import ImageDescribeRequest, ImageDescribeResponse
from app.services.describe_image_service import describe_image_service


@pytest.mark.asyncio
async def test_describe_image_service__forwards_provider_result_unchanged():
    payload = ImageDescribeRequest(
        image_url="https://example.com/image.jpg"
    )
    response = ImageDescribeResponse(
        title="Sunset Sail",
        description="A sailboat floating on calm water during sunset.",
        tags=["sailboat", "sunset", "water"]
    )
    mock_provider = AsyncMock()
    mock_provider.describe_image.return_value = response
    with patch(
        "app.services.describe_image_service.get_llm_provider",
        return_value=mock_provider
    ):
        result = await describe_image_service(payload)
    mock_provider.describe_image.assert_awaited_once_with(payload)
    assert result == response
