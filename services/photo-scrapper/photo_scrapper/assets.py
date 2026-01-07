from dagster import asset, get_dagster_logger

@asset
def hello_world():
    log = get_dagster_logger()
    log.info("Hello, Dagster!")
    return "Hello, Dagster!"