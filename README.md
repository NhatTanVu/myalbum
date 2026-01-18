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

## 🛠️ Technology Stack

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

# 🚀 Scheduled Photo Ingestion

## 📌 Description
MyAlbum now supports a Scheduled Photo Ingestion feature that automatically retrieves photos from external photo providers (like Pexels) based on configured categories and criteria. A scheduler enqueues ingestion tasks daily, a message queue buffers tasks, and a background worker retrieves and stores photos into the MyAlbum database without duplication. This enables automated enrichment of MyAlbum content and decouples scraping logic from the core API.

## 🧠 Architecture
```
 +------------------+       +----------------------+       +------------------+
 |   Dagster        |       | Azure Service Bus    |       |  C# Worker       |
 | (Scheduler/      |  -->  | (Task Queue / DLQ)   |  -->  | (.NET, Queue     |
 |  Orchestrator)   |       |                      |       |  Processing)     |
 +------------------+       +----------------------+       +------------------+
                                                              │         │
                                                              ▼         │
                                           ┌───────────────────────┐    │
                                           │ WebSPA.React.Identity |    │
                                           │ (Generate JWT Token)  |    │
                                           └───────────────────────┘    │
                                                                        │ JWT
                                                                        ▼
                                                         ┌──────────────────────┐
                                                         │    Microservices     │
                                                         │                      │
                                                         │ ┌──────────────────┐ │
                                                         │ │ Photos Service   | |
                                                         | |  (C#, .NET Core) | |
                                                         │ └──────────────────┘ |
                                                         └──────────────────────┘
```

## 🛠️ Tech Stack
| Component | Technology |
|------|----------------|
| Scheduling & Orchestration | Python, Dagster |
| Task Queue | Azure Service Bus |
| Worker / Processing | C# (.NET 10) Worker |
| Photo Provider APIs | Pexels (extendable to Flickr, etc.) |
| Photo API | MyAlbum Photo API (ASP.NET Core) |
| Storage | MyAlbum Database |
| Deployment (Dev) | Docker / Docker Compose |
| Deployment (Cloud) | Azure Container Apps / Azure Services |

# 🎥 Screenshots
1. **Explore photos and albums**\ (Click on the thumbnail to view the video)
[![My Album - Explore](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/explore_album.jpg?raw=true)](https://www.youtube.com/watch?v=z1c7Vs1JODE)
2. **AI features**
   1. Auto-generate image title (after Login and Select Edit Photo)
      <kbd>![Edit Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/edit_photo.png?raw=true)</kbd>
   2. Suggest friendly comments (after Login and add hint to New Comment textbox)
      <kbd>![New Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/new_comment_AI.jpg?raw=true)</kbd>
# Supporters :clap:
Thanks to everyone who has supported this project through ideas, feedback, and testing ❤️
[![Stargazers repo roster for @NhatTanVu/myalbum](http://reporoster.com/stars/NhatTanVu/myalbum)](https://github.com/NhatTanVu/myalbum/stargazers)
