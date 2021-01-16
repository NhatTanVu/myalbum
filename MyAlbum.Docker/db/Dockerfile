FROM mcr.microsoft.com/mssql/server:2019-latest
COPY sql /app
COPY entrypoint.sh /app
WORKDIR /app
EXPOSE 1433/tcp
CMD /bin/bash ./entrypoint.sh