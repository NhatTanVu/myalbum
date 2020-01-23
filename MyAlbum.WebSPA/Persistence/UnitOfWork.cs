using System.Threading.Tasks;
using MyAlbum.Persistence;
using MyAlbum.Core;

namespace MyAlbum.Persistence
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