from unittest.mock import patch


def test_image_describe__returns_service_response_unchanged(client):
    service_response = {
        "title": "Sunset Sail",
        "description": "A sailboat floating on calm water during sunset.",
        "tags": ["sailboat", "sunset", "water"]
    }

    with patch(
        "app.api.image.describe_image_service",
        return_value=service_response
    ):
        response = client.post(
            "/image/describe",
            json={
                "image_url": "https://example.com/image.jpg"
            }
        )

    assert response.status_code == 200
    assert response.json() == service_response

def test_image_describe__service_exception_returns_500(client):
    with patch(
        "app.api.image.describe_image_service",
        side_effect=RuntimeError("AI provider failure")
    ):
        response = client.post(
            "/image/describe",
            json={
                "image_url": "https://example.com/image.jpg"
            }
        )

    assert response.status_code == 500