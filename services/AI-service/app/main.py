from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api import image, comment, health
from app.config import settings

app = FastAPI(title="MyAlbum AI Service")

allow_origins = settings.CORS_ORIGINS

allow_credentials = True
if not allow_origins or "*" in allow_origins:
    allow_credentials = False

app.add_middleware(
    CORSMiddleware,
    allow_origins=allow_origins,
    allow_credentials=allow_credentials,
    allow_methods=["*"],        # GET, POST, OPTIONS, etc.
    allow_headers=["*"],        # Content-Type, Authorization, etc.
)

app.include_router(health.router)
app.include_router(image.router, prefix="/image", tags=["image"])
app.include_router(comment.router, prefix="/comment", tags=["comment"])
