FROM mcr.microsoft.com/dotnet/sdk:3.1-buster as publish
WORKDIR /src
# update 
RUN apt-get update
# install curl 
RUN apt-get install curl
# get install script and pass it to execute: 
RUN curl -sL https://deb.nodesource.com/setup_15.x | bash
# and install node 
RUN apt-get install -y nodejs
# and install npm
RUN apt-get install -y npm; exit 0
RUN npm install; exit 0
# confirm that it was successful 
RUN node -v
# npm installs automatically 
RUN npm -v
# copy project files
COPY ./WebSPA.Slim .
RUN dotnet restore
RUN dotnet publish WebSPA.Slim.csproj -c Release -o /app
# cleanup unused publish files
RUN rm -rf "/app/runtimes/linux-x86"
RUN rm -rf "/app/runtimes/osx-x64"
RUN rm -rf "/app/runtimes/win"
RUN rm -rf "/app/runtimes/win-arm64"
RUN rm -rf "/app/runtimes/win-x64"
RUN rm -rf "/app/runtimes/win-x86"
RUN rm -rf "/app/cs"
RUN rm -rf "/app/de"
RUN rm -rf "/app/es"
RUN rm -rf "/app/fr"
RUN rm -rf "/app/it"
RUN rm -rf "/app/ja"
RUN rm -rf "/app/ko"
RUN rm -rf "/app/pl"
RUN rm -rf "/app/pt-BR"
RUN rm -rf "/app/ru"
RUN rm -rf "/app/tr"
RUN rm -rf "/app/zh-Hans"
RUN rm -rf "/app/zh-Hant"

FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim as final
WORKDIR /app
COPY --from=publish /app /app
ENTRYPOINT [ "dotnet", "WebSPA.Slim.dll" ]