<p align="center"><a href="https://github.com/NhatTanVu/myalbum"><img src="https://github.com/NhatTanVu/myalbum/raw/master/MyAlbum.WebSPA/wwwroot/logo.jpg" alt="My Album logo" width="70"/></a></p>
<h1 align="center">My Album</h1>
<p align="center">Website to share albums and photos with Google Maps locations</p>

<p align="center"><a href="https://my-album.azurewebsites.net/"><img src="https://github.com/NhatTanVu/myalbum/raw/master/_screenshots/explore.JPG" alt="My Album demo" width="500"/></a></p>

# Status
[![Codecov](https://codecov.io/gh/NhatTanVu/myalbum/branch/master/graph/badge.svg)](https://codecov.io/gh/NhatTanVu/myalbum)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/4b7m4xj6fu82xtgn/branch/master?svg=true)](https://ci.appveyor.com/project/NhatTanVu/myalbum/branch/master)

[![Stargazers repo roster for @NhatTanVu/myalbum](https://reporoster.com/stars/NhatTanVu/myalbum)](https://github.com/NhatTanVu/myalbum/stargazers)
# Azure website
* **URL**: https://my-album.azurewebsites.net/
# Screenshots
1. **Explore**\
![Explore](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/explore.JPG?raw=true)

2. **World Map**\
![World Map](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/world_map.jpg?raw=true)

3. **Add Photo**\
![Add Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/add_photo.jpg?raw=true)

4. **Edit Photo**\
![Edit Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/edit_photo.jpg?raw=true)

5. **View Photo**\
![View Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo.JPG?raw=true)

6. **View Photo >> Object Detection** (click on the photo)
![View Photo >> Object Detection](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo_object_detection.JPG?raw=true)

7. **View Photo >> Add Comment**
![View Photo >> Add Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/add_comment.JPG?raw=true)

8. **View Photo >> Notify Comment (real-time)**
![View Photo >> Update New Comment (real-time)](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/notify_comment.jpg?raw=true)

9. **View Photo >> Reply Comment**
![View Photo >> Add & View Reply](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/reply_comment.jpg?raw=true)
# Deploy to Azure
1. Create 1 Azure App Service and 1 Azure SQL database
2. Add 2 app settings: "**ASPNETCORE_ENVIRONMENT**" and "**ConnectionStrings:Default**" to Azure App Service as below:
![App Settings](https://raw.githubusercontent.com/NhatTanVu/vega/master/_screenshots/Add%20App%20Settings.PNG)
3. Follow this [article](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1) to deploy to Azure
