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
* **URL**: https://my-album.azurewebsites.net/
* **Email**: guest@gmail.com (for add/edit photo, comment and reply)
* **Password**: 2u)TAa
* **Identity API**: https://my-album.azurewebsites.net/swagger/index.html
* **Album API**: https://my-album-album-api.azurewebsites.net/swagger/index.html
* **Photo API**: https://my-album-photo-api.azurewebsites.net/swagger/index.html
* **Comment API**: https://my-album-comment-api.azurewebsites.net/swagger/index.html
# Run in VS Code
1. Setup DB by running 3 scripts in "**src/WebSPA/sql**"
2. Change **Default** connection string in "**src/WebSPA/appsettings.json**"
3. Open workspace and press F5 to start Debugging
# Run in Docker
0. [Install](https://docs.docker.com/docker-for-windows/install/) Docker.
1. Open "**src/Docker**" folder and run: 
```
docker-compose down
docker-compose build
docker-compose up
```
2. Browse the website at https://localhost/
# Deploy to Azure
1. Create 1 Azure App Service and 1 Azure SQL database
2. Add 2 app settings: "**ASPNETCORE_ENVIRONMENT**" and "**ConnectionStrings:Default**" to Azure App Service:
<kbd>![App Settings](https://raw.githubusercontent.com/NhatTanVu/vega/master/_screenshots/Add%20App%20Settings.PNG)</kbd>
3. [Deploy](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1) to Azure
# Screenshots
1. **Explore**\
<kbd>![Explore](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/explore.JPG?raw=true)</kbd>

2. **Explore Album**\
<kbd>![Explore Album](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/explore_album.jpg?raw=true)</kbd>

3. **World Map**\
<kbd>![World Map](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/world_map.jpg?raw=true)</kbd>

4. **Add Photo**\
<kbd>![Add Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/add_photo.jpg?raw=true)</kbd>

5. **Edit Photo**\
<kbd>![Edit Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/edit_photo.jpg?raw=true)</kbd>

6. **View Photo**\
<kbd>![View Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/view_photo.JPG?raw=true)</kbd>

7. **View Photo >> Object Detection** (click on the photo)
<kbd>![View Photo >> Object Detection](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/view_photo_object_detection.JPG?raw=true)</kbd>

8. **View Photo >> Add Comment**
<kbd>![View Photo >> Add Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/add_comment.JPG?raw=true)</kbd>

9. **View Photo >> Notify Comment (real-time)**
<kbd>![View Photo >> Update New Comment (real-time)](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/notify_comment.jpg?raw=true)</kbd>

10. **View Photo >> Reply Comment**
<kbd>![View Photo >> Add & View Reply](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/screenshots/reply_comment.jpg?raw=true)</kbd>
