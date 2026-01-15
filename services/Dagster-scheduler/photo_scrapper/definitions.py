from dagster import Definitions
from photo_scrapper.assets import enqueue_photo_ingestion_tasks

defs = Definitions(
    assets=[enqueue_photo_ingestion_tasks]
)
