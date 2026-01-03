from openai import AsyncOpenAI
from app.providers.LLM.base import LLMProvider
from app.config import settings
import json
from app.schemas.image import ImageDescribeRequest, ImageDescribeResponse
from app.schemas.comment import CommentSuggestRequest, CommentSuggestResponse


class OpenAIProvider(LLMProvider):
    def __init__(self):
        self.client = AsyncOpenAI(api_key=settings.OPENAI_API_KEY)

    async def describe_image(self, request: ImageDescribeRequest) -> ImageDescribeResponse:
        prompt = (
            "Describe this image. Return a short title, a concise description, "
            "and 3–5 relevant tags. Respond in JSON format with keys: "
            "title, description, tags."
        )
        response = await self.client.chat.completions.create(
            model=settings.OPENAI_MODEL,
            response_format={"type": "json_object"},
            messages=[
                {
                    "role": "user",
                    "content": [
                        {"type": "text", "text": prompt},
                        {"type": "image_url", "image_url": {
                            "url": request.image_url}}
                    ]
                }
            ],
            timeout=settings.OPENAI_TIMEOUT,
            temperature=0.3
        )

        try:
            result_str = response.choices[0].message.content
            if result_str is None:
                raise ValueError(
                    "Failed to parse OpenAI response: content is None")
            result_json = json.loads(result_str)
            return ImageDescribeResponse(
                title=result_json.get("title", ""),
                description=result_json.get("description", ""),
                tags=result_json.get("tags", [])
            )
        except (json.JSONDecodeError, IndexError) as e:
            raise ValueError(f"Failed to parse OpenAI response: {e}")

    async def suggest_comment(self, request: CommentSuggestRequest) -> CommentSuggestResponse:
        system_prompt = (
            "You generate friendly, natural comments for social media images. "
            "Always follow the user's rules and output valid JSON only."
        )

        user_prompt = f"""
        You are given:
        1) An image
        2) A list of user hints

        The hints may express:
        - topics (e.g. "sunset", "family")
        - or sentiment (e.g. "I don't like this", "looks bad", "beautiful")

        Your task:
        Generate 3–5 short comments suitable for a social media post.

        Rules:
        - First, infer the sentiment from the hints (positive, neutral, or negative).
        - If hints express dislike, comments should clearly express dissatisfaction.
        - Avoid softening negative opinions into neutral praise.
        - If the hints are positive or neutral, comments should be friendly and positive.
        - Each comment must clearly reflect at least one hint.
        - Do NOT ignore negative wording such as "don't like", "bad", "boring".
        - One sentence per comment.
        - Natural, human tone.
        - At most one emoji per comment.
        - No hashtags.
        - Output valid JSON only.

        Hints:
        {", ".join(request.hints)}

        Output format:
        {{
        "comments": ["...", "..."]
        }}
        """

        response = await self.client.chat.completions.create(
            model=settings.OPENAI_MODEL,
            response_format={"type": "json_object"},
            temperature=0.6,
            timeout=settings.OPENAI_TIMEOUT,
            messages=[
                {"role": "system", "content": system_prompt},
                {
                    "role": "user",
                    "content": [
                        {"type": "text", "text": user_prompt},
                        {
                            "type": "image_url",
                            "image_url": {"url": request.image_url}
                        }
                    ]
                }
            ]
        )

        try:
            result_str = response.choices[0].message.content
            if result_str is None:
                raise ValueError(
                    "Failed to parse OpenAI response: content is None")
            result_json = json.loads(result_str)
            return CommentSuggestResponse(
                comments=result_json.get("comments", [])
            )
        except (json.JSONDecodeError, IndexError) as e:
            raise ValueError(f"Failed to parse OpenAI response: {e}")
