from pydantic import BaseModel
from typing import List

class ImageDescribeRequest(BaseModel):
    image_url: str

class ImageDescribeResponse(BaseModel):
    title: str
    description: str
    tags: List[str]