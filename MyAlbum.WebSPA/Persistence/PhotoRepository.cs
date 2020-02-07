using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Photo>> GetPhotos(PhotoQuery filter)
        {
            IQueryable<Photo> query = this.context.Photos
                .Include(p => p.Comments)
                .Include(p => p.PhotoCategories)
                .AsQueryable();
            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.PhotoCategories.Select(pc => pc.CategoryId).Any(id => id == filter.CategoryId));
            var photos = await query.ToListAsync();
            return photos;
        }
    }
}