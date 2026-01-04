<p align="center"><a href="https://github.com/NhatTanVu/myalbum"><kbd><img src="https://github.com/NhatTanVu/myalbum/raw/master/src/WebSPA/wwwroot/logo.jpg" alt="My Album logo" width="70"/></kbd></a></p>
<h1 align="center">My Album</h1>
<p><b>MyAlbum</b> is a modern web application for <b>sharing photo albums and individual photos</b>, enriched with <b>location context via Google Maps</b> and <b>AI-powered assistance</b>.</p>
Users can:
<ul>
<li>Create and share albums</li>
<li>Upload and browse photos
<li>View photo locations on <b>Google Maps</b>
<li>Use an <b>AI assistant</b> to:
   <ol>
      <li>Auto-generate image titles and descriptions</li>
      <li>Suggest friendly comments based on user-provided hints</li>
   </ol>
</li>
</ul>

The system is built using a **microservices architecture**, designed to be scalable, extensible, and production-ready.

# 🚦Status (Active Development)
[![Codecov](https://codecov.io/gh/NhatTanVu/myalbum/branch/master/graph/badge.svg)](https://codecov.io/gh/NhatTanVu/myalbum)
[![AppVeyor](http://ci.appveyor.com/api/projects/status/4b7m4xj6fu82xtgn/branch/master?svg=true)](https://ci.appveyor.com/project/NhatTanVu/myalbum/branch/master)

> ⚠️ **This project is under active development**

- Core album and photo features are functional
- AI service is live and evolving
- Architecture is stable, but APIs and UI may continue to change

✅ **Current focus**
- Improving AI-assisted features
- Strengthening CI/CD and deployment reliability
- Enhancing user experience and performance

## 🧠 Architecture
```
┌─────────────────────────────┐
│        Web Browser          │
│         (React UI)          │
└──────────────┬──────────────┘
               │ HTTPS
               ▼
┌─────────────────────────────┐
│    WebSPA.React.Identity    │
│ - Serves frontend UI        │
│ - Authentication            │
│ - Issues JWT tokens         │
└──────────────┬──────────────┘
               │ JWT
               ▼
┌───────────────────────────────────────────────────────────────────────────────────────┐
│                                   Microservices                                       │
│                                                                                       │
│ ┌────────────────┐ ┌────────────────┐ ┌──────────────────┐ ┌────────────────────────┐ │
│ │ Albums Service │ │ Photos Service │ │ Comments Service | |       AI Service       | |
| |(C#, .NET Core) | |(C#, .NET Core) | |  (C#, .NET Core) | | (Python, FastAPI + LLM)| |
│ └────────────────┘ └────────────────┘ └──────────────────┘ └────────────────────────┘ |
└───────────────────────────────────────────────────────────────────────────────────────┘
```
### 🔐 Identity & Authentication

**WebSPA.React.Identity** acts as both:
- The **frontend entry point**
- The **authentication authority**

Responsibilities:
- Serves React frontend requests
- Handles user authentication
- Issues **JWT tokens**
- Acts as a gateway for downstream API calls

All downstream services validate JWT tokens issued by WebSPA.React.Identity.

---

### 🧩 Backend Microservices

Each backend service is:
- Independently deployable
- Stateless
- Protected by JWT
- Documented with **Swagger / OpenAPI**

| Service | Responsibility |
|------|----------------|
| Albums | Album management |
| Photos | Photo upload, metadata, and location |
| Comments | User comments on photos |
| **AI Service** | Image description & comment suggestion |

---

### 🤖 AI Service Architecture

```
┌──────────────────────────┐
│ React Frontend           │
│ (Generate / Suggest UI)  │
└────────────┬─────────────┘
             │ HTTPS + JWT
             ▼
┌──────────────────────────┐
│     AI Service API       │
│        (FastAPI)         │
│                          │
│ /image/describe          │
│ /comment/suggest         │
│                          │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│ LLM Provider Layer       │
│ (Pluggable Design)       │
│                          │
│ - OpenAI (current)       │
│ - Future providers       │
└──────────────────────────┘
```

**Key design goals**
- Provider-agnostic LLM layer
- Easy model switching
- Clean separation between API, prompts, and providers

The AI service enhances user experience by automating repetitive and creative tasks:
- **Image description**
  - Auto-generate title and description from image content
- **Comment suggestion**
  - Generate friendly comments based on user hints (positive or negative)
- **Extensible architecture**
  - Designed to support multiple LLM providers in the future

# 🌐 Website
* **URL**: 
   1. https://my-album.azurewebsites.net/ (Angular 8.0)
   2. https://my-album-react.azurewebsites.net/ (React 16.12)
* **Email**: guest@gmail.com (for add/edit photo, album, comment and reply)
* **Password**: 2u)TAa
* **Albums API**: https://my-album-album-api.azurewebsites.net/swagger/index.html
* **Photos API**: https://my-album-photo-api.azurewebsites.net/swagger/index.html
* **Comments API**: https://my-album-comment-api.azurewebsites.net/swagger/index.html
* **AI Service API**: https://my-album-ai.azurewebsites.net/docs

## 🛠 Technology Stack

### Frontend
- React
- TypeScript

### Backend
- .NET Core + SignalR + Entity Framework (Albums, Photos, Comments)
- FastAPI (AI Service)

### AI
- OpenAI (current provider)
- Pluggable provider design

### Infrastructure
- Azure App Service
- GitHub Actions (CI/CD)
 
# Run in Microsoft Visual Studio Community 2019
1. Setup DB by running 3 scripts in "**src/WebSPA/sql**"
2. Open "**src/MyAlbum.sln**"
3. Change **Default** connection string in either:
   1. **React** with: "**src/WebSPA.React.Identity/appsettings.Development.json**"
   2. **Angular** with: "**src/WebSPA.Identity/appsettings.Development.json**"
4. Set Startup Projects using menu "**Debug->Set Startup Projects...**" for Debugging in either:
   1. **React** with projects: **Web Apps/WebSPA.React.Identity** and 3 projects in Services folder
   2. **Angular** with projects: **Web Apps/WebSPA.Identity** and 3 projects in Services folder
5. Press F5 for Debugging

# Run in Docker
0. [Install](https://docs.docker.com/docker-for-windows/install/) Docker.
1. Open "**src/Docker**" folder and run: 
```
docker-compose down
docker-compose build
docker-compose up
```
2. Open "**src/Docker**" folder and [install](https://www.thewindowsclub.com/manage-trusted-root-certificates-windows) this SSL certificate to Local Computer's "**Trusted Root Certification Authorities**" folder:
```
File name: my-album.pfx
Password: 2u)TAa
```
3. Verify by browsing https://localhost:5002/swagger/ successfully.
4. Browse the website at http://localhost:5000/

# Deploy to Azure
1. Create 1 Azure App Service and 1 Azure SQL database
2. Add 2 app settings: "**ASPNETCORE_ENVIRONMENT**" and "**ConnectionStrings:Default**" to Azure App Service:
<kbd>![App Settings](https://raw.githubusercontent.com/NhatTanVu/vega/master/_screenshots/Add%20App%20Settings.PNG)</kbd>
3. [Deploy](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1) to Azure

# Screenshots
1. **[Photo] Explore**\
<kbd>![Explore Photos](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/explore.JPG?raw=true)</kbd>

2. **[Photo] World Map**\
<kbd>![World Map](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/world_map.jpg?raw=true)</kbd>

3. **[Album] Explore**\
<kbd>![Explore Albums](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/explore_album.jpg?raw=true)</kbd>

4. **[Album] View**\
<kbd>![View Album](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/view_album.jpg?raw=true)</kbd>

5. **[Photo] Add**\
<kbd>![Add Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/add_photo.jpg?raw=true)</kbd>

6. **[Photo] Edit**\
<kbd>![Edit Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/edit_photo.jpg?raw=true)</kbd>

7. **[Photo] View**\
<kbd>![View Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/view_photo.JPG?raw=true)</kbd>

8. **[Photo] View >> Object Detection** (click on the photo)\
<kbd>![View Photo >> Object Detection](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/view_photo_object_detection.JPG?raw=true)</kbd>

9. **[Photo] View >> Add Comment**
<kbd>![View Photo >> Add Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/add_comment.JPG?raw=true)</kbd>

10. **[Photo] View >> Notify Comment (real-time)**\
<kbd>![View Photo >> Update New Comment (real-time)](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/notify_comment.jpg?raw=true)</kbd>

11. **[Photo] View >> Reply Comment**\
<kbd>![View Photo >> Add & View Reply](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/reply_comment.jpg?raw=true)</kbd>

# Supporters :clap:
Thanks to everyone who has supported this project through ideas, feedback, and testing ❤️
[![Stargazers repo roster for @NhatTanVu/myalbum](http://reporoster.com/stars/NhatTanVu/myalbum)](https://github.com/NhatTanVu/myalbum/stargazers)
