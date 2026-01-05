import pytest
from unittest.mock import AsyncMock, patch
from app.schemas.comment import CommentSuggestRequest, CommentSuggestResponse
from app.services.suggest_comment_service import suggest_comment_service


@pytest.mark.asyncio
async def test_suggest_comment_service__forwards_provider_result_unchanged():
    payload = CommentSuggestRequest(
        image_url="https://example.com/image.jpg",
        hints=["I don't like this photo"]
    )
    response = CommentSuggestResponse(
        comments=[
            "Not really my favorite shot.",
            "The composition feels off.",
            "I don't connect with this photo.",
        ]
    )
    mock_provider = AsyncMock()
    mock_provider.suggest_comment.return_value = response
    with patch(
        "app.services.suggest_comment_service.get_llm_provider",
        return_value=mock_provider
    ):
        result = await suggest_comment_service(payload)
    mock_provider.suggest_comment.assert_awaited_once_with(payload)
    assert result == response
