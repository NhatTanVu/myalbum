from dagster import Definitions
from photo_scrapper.assets import hello_world

defs = Definitions(
    assets=[hello_world]
)
