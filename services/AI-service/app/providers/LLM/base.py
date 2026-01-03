from abc import ABC, abstractmethod

from app.schemas.comment import CommentSuggestRequest, CommentSuggestResponse
from app.schemas.image import ImageDescribeRequest, ImageDescribeResponse


class LLMProvider(ABC):
    @abstractmethod
    async def describe_image(self, request: ImageDescribeRequest) -> ImageDescribeResponse:
        pass

    @abstractmethod
    async def suggest_comment(self, request: CommentSuggestRequest) -> CommentSuggestResponse:
        pass
