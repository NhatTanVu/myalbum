using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MyAlbum.Services.Indentity.API.Areas.Identity.IdentityHostingStartup))]
namespace MyAlbum.Services.Indentity.API.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}