import logging
from fastapi import Depends, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from jose import jwt, JWTError
import httpx
from app.config import settings

AUTHORITY = settings.IDENTITY_URL
AUDIENCES = [
    "MyAlbum.DeveloperAPI",
    "Identity.APIAPI",
    "WebSPA.IdentityAPI",
    "WebSPA.React.IdentityAPI"
]
ALGORITHMS = ["RS256", "rsa-sha256"]

logger = logging.getLogger(__name__)
security = HTTPBearer()

_jwks_cache = None


async def get_jwks():
    global _jwks_cache
    if _jwks_cache:
        return _jwks_cache

    async with httpx.AsyncClient() as client:
        oidc = await client.get(f"{AUTHORITY}/.well-known/openid-configuration")
        jwks_uri = oidc.json()["jwks_uri"]

        jwks = await client.get(jwks_uri)
        _jwks_cache = jwks.json()

    return _jwks_cache


async def get_current_user(
    credentials: HTTPAuthorizationCredentials = Depends(security),
):
    token = credentials.credentials

    try:
        jwks = await get_jwks()

        payload = jwt.decode(
            token,
            jwks,
            algorithms=ALGORITHMS,
            options={"verify_aud": False},
            issuer=AUTHORITY,
        )

        aud = payload.get("aud")
        logger.info(f"get_current_user - aud:{aud}")
        if isinstance(aud, str):
            aud = {aud}
        elif isinstance(aud, list):
            aud = set(aud)
        else:
            aud = set()

        if not aud & set(AUDIENCES):
            raise HTTPException(status_code=401, detail="Invalid audience")

        return payload  # claims

    except JWTError as e:
        # ✅ Expected JWT failures (expired, invalid aud, bad sig, etc.)
        logger.warning(
            f"""JWT validation failed:
                "error": {str(e)},
                "issuer": {AUTHORITY},
                "audience": {AUDIENCES},
            """
        )
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid or expired token",
        )
    except Exception as e:
        # ❗ Unexpected errors (misconfig, network, JWKS issues)
        logger.exception("Unexpected error during JWT authentication")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail="Authentication service error",
        )
