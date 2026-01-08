import json
from dagster import asset, get_dagster_logger
import yaml
from azure.servicebus import ServiceBusClient, ServiceBusMessage
from dotenv import load_dotenv

# Load .env file once at import time
load_dotenv()


@asset
def enqueue_photo_ingestion_tasks():
    """
    Read config.yaml and enqueues one Service Bus message
    per photo category.
    """
    log = get_dagster_logger()

    with open("config.yaml") as f:
        config = yaml.safe_load(f)

    categories = config["categories"]
    photos_per_category = config["photos_per_category"]
    criteria = config["criteria"]
    queue_name = config["service_bus"]["queue_name"]
    providers = config["providers"]

    import os
    conn_str = os.environ["SERVICE_BUS_CONNECTION_STRING"]

    with ServiceBusClient.from_connection_string(conn_str) as client:
        sender = client.get_queue_sender(queue_name=queue_name)
        with sender:
            for provider in providers:
                for category in categories:
                    payload = {
                        "category": category,
                        "photos_per_category": photos_per_category,
                        "criteria": criteria,
                        "provider": provider
                    }

                    sender.send_messages(
                        ServiceBusMessage(json.dumps(payload))
                    )

                    log.info(
                        f"Enqueued ingestion task for category: {category}, provider: {provider}"
                    )

    return {
        "categories_enqueued": len(categories),
        "queue": queue_name
    }
