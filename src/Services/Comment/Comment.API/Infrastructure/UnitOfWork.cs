using System.Threading.Tasks;
using MyAlbum.Services.Comment.API.Core;

namespace MyAlbum.Services.Comment.API.Infrastructure
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