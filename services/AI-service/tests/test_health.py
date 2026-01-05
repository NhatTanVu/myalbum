def test_health_status_ok(client):
    health_service_response = {
        "status": "ok"
    }

    response = client.get(
        "/health"
    )

    assert response.status_code == 200
    assert response.json() == health_service_response
