using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.EntityFrameworkCore;

namespace MyAlbum.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyAlbumDbContext context;
        public PhotoRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public void Add(Photo photo)
        {
            this.context.Add(photo);
        }

        public async Task<Photo> GetAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await context.Photos.FindAsync(id);
            else
                return await context.Photos
                    .Include(v => v.Author)
                    .Include(v => v.Album)
                    .Include(v => v.Comments)
                    .Include(v => v.PhotoCategories)
                    .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Photo>> GetPhotos()
        {
            var photos = await this.context.Photos.Include(p => p.Comments).Include(p => p.PhotoCategories).ToListAsync();
            return photos;
        }
    }
}