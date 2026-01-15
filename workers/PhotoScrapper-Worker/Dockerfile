# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ./*.csproj ./photoscrapper-worker/
RUN dotnet restore photoscrapper-worker/PhotoScrapper.Worker.csproj

COPY ./ ./photoscrapper-worker/
WORKDIR /src/photoscrapper-worker
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/runtime:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PhotoScrapper.Worker.dll"]
