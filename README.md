<p align="center"><a href="https://github.com/NhatTanVu/myalbum"><kbd><img src="https://github.com/NhatTanVu/myalbum/raw/master/src/WebSPA/wwwroot/logo.jpg" alt="My Album logo" width="70"/></kbd></a></p>
<h1 align="center">My Album</h1>
<p align="center">Website to share albums and photos with Google Maps locations</p>

<p align="center"><a href="https://my-album.azurewebsites.net/"><kbd><img src="https://github.com/NhatTanVu/myalbum/raw/master/screenshots/explore.JPG" alt="My Album demo" width="500"/></kbd></a></p>

# Status
[![Codecov](https://codecov.io/gh/NhatTanVu/myalbum/branch/master/graph/badge.svg)](https://codecov.io/gh/NhatTanVu/myalbum)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/4b7m4xj6fu82xtgn/branch/master?svg=true)](https://ci.appveyor.com/project/NhatTanVu/myalbum/branch/master)

# Supporters :clap:
[![Stargazers repo roster for @NhatTanVu/myalbum](https://reporoster.com/stars/NhatTanVu/myalbum)](https://github.com/NhatTanVu/myalbum/stargazers)
# Website
* **URL**: 
   1. https://my-album.azurewebsites.net/ (Angular 8.0)
   2. https://my-album-react.azurewebsites.net/ (React 16.12)
* **Email**: guest@gmail.com (for add/edit photo, album, comment and reply)
* **Password**: 2u)TAa
* **Identity API**: https://my-album.azurewebsites.net/swagger/index.html
* **Album API**: https://my-album-album-api.azurewebsites.net/swagger/index.html
* **Photo API**: https://my-album-photo-api.azurewebsites.net/swagger/index.html
* **Comment API**: https://my-album-comment-api.azurewebsites.net/swagger/index.html
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
2. Open "**src/Docker**" folder and <a href="https://www.thewindowsclub.com/manage-trusted-root-certificates-windows" target="_blank">install</a> this file to "**Trusted Root Certification Authorities**":
```
File name: my-album.pfx
Password: 2u)TAa
```
Verify by browsing https://localhost:5002/swagger/ successfully.

3. Browse the website at http://localhost:5000/
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
