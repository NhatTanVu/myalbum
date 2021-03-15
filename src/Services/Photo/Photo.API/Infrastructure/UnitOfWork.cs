using System.Threading.Tasks;
using MyAlbum.Services.Photo.API.Core;

namespace MyAlbum.Services.Photo.API.Infrastructure
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