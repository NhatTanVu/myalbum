using System.Threading.Tasks;
using MyAlbum.Services.Album.API.Core;

namespace MyAlbum.Services.Album.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAlbumDbContext context;
        public UnitOfWork(MyAlbumDbContext context)
        {
            this.context = context;

        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}