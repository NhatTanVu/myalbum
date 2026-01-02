from fastapi import FastAPI
from app.api import image, comment,health

app = FastAPI(title="MyAlbum AI Service")

app.include_router(health.router)
app.include_router(image.router, prefix="/image", tags=["image"])
app.include_router(comment.router, prefix="/comment", tags=["comment"])
