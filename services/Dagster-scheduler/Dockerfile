FROM python:3.11-slim

# Environment variables first (good practice)
ENV DAGSTER_HOME=/opt/dagster/dagster_home

# Create DAGSTER_HOME
RUN mkdir -p /opt/dagster/dagster_home

WORKDIR /app

COPY pyproject.toml poetry.lock ./

RUN pip install --no-cache-dir poetry \
 && poetry config virtualenvs.create false \
 && poetry install --no-interaction --no-ansi

# Copy Dagster config
COPY dagster.yaml /opt/dagster/dagster_home/dagster.yaml

COPY photo_scrapper /app/photo_scrapper

EXPOSE 3000

CMD ["dagster-webserver", "-h", "0.0.0.0", "-p", "3000"]
