from unittest.mock import patch


def test_comment_suggest_returns_provider_response_unchanged(client):
    provider_response = {
        "comments": [
            "Not really my favorite shot.",
            "The composition feels off.",
            "I don't connect with this photo."
        ]
    }

    with patch(
        "app.api.comment.suggest_comment_service",
        return_value=provider_response
    ):
        response = client.post(
            "/comment/suggest",
            json={
                "image_url": "https://example.com/image.jpg",
                "hints": ["I don't like this photo"]
            }
        )

    assert response.status_code == 200
    assert response.json() == provider_response
