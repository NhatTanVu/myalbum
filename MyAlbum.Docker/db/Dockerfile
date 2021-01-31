FROM mcr.microsoft.com/mssql/server:2019-latest
WORKDIR /app
ARG SSL_KEYSTORE_PASSWORD
USER root
COPY ./MyAlbum.WebSPA/sql .
COPY ./MyAlbum.Docker/db/entrypoint.sh .
# update 
RUN apt-get update
# doc2unix
RUN apt-get install dos2unix
# convert entrypoint.sh
RUN dos2unix ./entrypoint.sh
EXPOSE 1433/tcp
CMD /bin/bash ./entrypoint.sh