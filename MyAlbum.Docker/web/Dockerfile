FROM mcr.microsoft.com/dotnet/sdk:3.1
RUN apt-get update && apt-get install -y libgdiplus
RUN apt-get update && apt-get install -y libgomp1
COPY publish /app
COPY entrypoint.sh /app
WORKDIR /app
#EXPOSE 80/tcp 443/tcp
CMD /bin/bash ./entrypoint.sh