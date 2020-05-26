# My Album
Website to share albums, photos with Google Maps locations
# Deploy to Azure
1. Create 1 Azure App Service and 1 Azure SQL database
2. Add 2 app settings: "**ASPNETCORE_ENVIRONMENT**" and "**ConnectionStrings:Default**" to Azure App Service as below:
![App Settings](https://raw.githubusercontent.com/NhatTanVu/vega/master/_screenshots/Add%20App%20Settings.PNG)
3. Follow this [article](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-3.1) to deploy to Azure
# Azure website
* **URL**: https://my-album.azurewebsites.net/
# Screenshots
1. **Photo List**\
![Photo List](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/photo_list.JPG?raw=true)

2. **New Photo**\
![New Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/new_photo.JPG?raw=true)

3. **View Photo**\
![View Photo](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo.JPG?raw=true)

4. **View Photo >> Object Detection** (click on the photo)
![View Photo >> Object Detection](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/view_photo_object_detection.JPG?raw=true)

5. **View Photo >> Add Comment** (click on the photo)
![View Photo >> Add Comment](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/add_comment.JPG?raw=true)

6. **View Photo >> Update New Comment (real-time)**
![View Photo >> Update New Comment (real-time)](https://raw.githubusercontent.com/NhatTanVu/myalbum/master/_screenshots/update_new_comment_real_time.JPG?raw=true)
