import pytest
from fastapi.testclient import TestClient
from app.main import app
from app.security.jwt import get_current_user


def fake_get_current_user():
    return {
        "sub": "test-user-id",
        "scope": "ai",
    }


@pytest.fixture(autouse=True)
def override_auth_dependency():
    app.dependency_overrides[get_current_user] = fake_get_current_user
    yield
    app.dependency_overrides.clear()


@pytest.fixture
def client():
    return TestClient(app)
