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