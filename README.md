<p align="center"><a href="https://github.com/NhatTanVu/myalbum"><kbd><img src="https://github.com/NhatTanVu/myalbum/raw/master/MyAlbum.WebSPA/wwwroot/logo.jpg" alt="My Album logo" width="70"/></kbd></a></p>
<h1 align="center">My Album</h1>
<p align="center">Website to share albums and photos with Google Maps locations</p>

<p align="center"><a href="https://my-album.azurewebsites.net/"><kbd><img src="https://github.com/NhatTanVu/myalbum/raw/master/_screenshots/explore.JPG" alt="My Album demo" width="500"/></kbd></a></p>

# Status
[![Codecov](https://codecov.io/gh/NhatTanVu/myalbum/branch/master/graph/badge.svg)](https://codecov.io/gh/NhatTanVu/myalbum)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/4b7m4xj6fu82xtgn/branch/master?svg=true)](https://ci.appveyor.com/project/NhatTanVu/myalbum/branch/master)

# Supporters :clap:
[![Stargazers repo roster for @NhatTanVu/myalbum](https://reporoster.com/stars/NhatTanVu/myalbum)](https://github.com/NhatTanVu/myalbum/stargazers)
# Website
* **URL**: https://my-album.azurewebsites.net/
* **Email**: guest@gmail.com (for add/edit photo, comment and reply)
* **Password**: 2u)TAa
# Screenshots
1. **Explore**\
<kbd>![Explore](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/explore.JPG?raw=true)</kbd>

2. **World Map**\
<kbd>![World Map](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/world_map.jpg?raw=true)</kbd>

3. **Add Photo**\
<kbd>![Add Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/add_photo.jpg?raw=true)</kbd>

4. **Edit Photo**\
<kbd>![Edit Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/edit_photo.jpg?raw=true)</kbd>

5. **View Photo**\
<kbd>![View Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo.JPG?raw=true)</kbd>

6. **View Photo >> Object Detection** (click on the photo)
<kbd>![View Photo >> Object Detection](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo_object_detection.JPG?raw=true)</kbd>

7. **View Photo >> Add Comment**
<kbd>![View Photo >> Add Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/add_comment.JPG?raw=true)</kbd>

8. **View Photo >> Notify Comment (real-time)**
<kbd>![View Photo >> Update New Comment (real-time)](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/notify_comment.jpg?raw=true)</kbd>

9. **View Photo >> Reply Comment**
<kbd>![View Photo >> Add & View Reply](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/reply_comment.jpg?raw=true)</kbd>
# Deployment to Azure
1. Create 1 Azure App Service and 1 Azure SQL database
2. Add 2 app settings: "**ASPNETCORE_ENVIRONMENT**" and "**ConnectionStrings:Default**" to Azure App Service as below:
<kbd>![App Settings](https://raw.githubusercontent.com/NhatTanVu/vega/master/_screenshots/Add%20App%20Settings.PNG)</kbd>
3. Follow this [article](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1) to deploy to Azure
