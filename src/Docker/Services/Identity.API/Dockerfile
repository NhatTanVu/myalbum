FROM mcr.microsoft.com/dotnet/sdk:3.1-buster as publish
WORKDIR /src
# copy project files
COPY ./Services/Identity/Identity.API .
RUN dotnet restore
RUN dotnet publish Identity.API.csproj -c Release -o /app
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
# install certificate tool locally
RUN dotnet tool install --tool-path /app dotnet-certificate-tool

FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim as final
WORKDIR /app
COPY --from=publish /app /app
# trust /app/my-album.pfx certificate
RUN /app/certificate-tool add --file /app/my-album.pfx --password "2u)TAa" -s "Root"
ENTRYPOINT [ "dotnet", "Identity.API.dll" ]