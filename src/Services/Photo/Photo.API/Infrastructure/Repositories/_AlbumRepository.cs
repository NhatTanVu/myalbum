using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Services.Photo.API.Core;
using Models = MyAlbum.Services.Photo.API.Core.Models;

namespace MyAlbum.Services.Photo.API.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MyAlbumDbContext context;

        public AlbumRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<Models.Album> GetAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await context.Albums.FindAsync(id);
            else
            {
                var album = await context.Albums
                    .Include(p => p.Author)
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return album;
            }
        }
    }
}