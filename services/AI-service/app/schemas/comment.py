from pydantic import BaseModel
from typing import List

class CommentSuggestRequest(BaseModel):
    image_url: str
    hints: List[str]

class CommentSuggestResponse(BaseModel):
    comments: List[str]