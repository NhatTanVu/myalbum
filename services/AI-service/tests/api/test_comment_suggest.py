from unittest.mock import patch


def test_comment_suggest__returns_service_response_unchanged(client):
    service_response = {
        "comments": [
            "Not really my favorite shot.",
            "The composition feels off.",
            "I don't connect with this photo."
        ]
    }

    with patch(
        "app.api.comment.suggest_comment_service",
        return_value=service_response
    ):
        response = client.post(
            "/comment/suggest",
            json={
                "image_url": "https://example.com/image.jpg",
                "hints": ["I don't like this photo"]
            }
        )

    assert response.status_code == 200
    assert response.json() == service_response

def test_comment_suggest__service_exception_returns_500(client):
    with patch(
        "app.api.comment.suggest_comment_service",
        side_effect=RuntimeError("AI provider failure")
    ):
        response = client.post(
            "/comment/suggest",
            json={
                "image_url": "https://example.com/image.jpg",
                "hints": ["I don't like this photo"]
            }
        )

    assert response.status_code == 500