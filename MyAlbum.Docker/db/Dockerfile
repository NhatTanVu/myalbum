FROM mcr.microsoft.com/mssql/server:2019-latest
WORKDIR /app
COPY ./MyAlbum.WebSPA/sql .
COPY ./MyAlbum.Docker/db/entrypoint.sh .
EXPOSE 1433/tcp
CMD /bin/bash ./entrypoint.sh