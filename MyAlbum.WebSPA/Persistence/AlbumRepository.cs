using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MyAlbum.Persistence
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MyAlbumDbContext context;
        private readonly IPhotoRepository photoRepository;

        public AlbumRepository(MyAlbumDbContext context, IPhotoRepository photoRepository)
        {
            this.context = context;
            this.photoRepository = photoRepository;
        }

        public virtual void Add(Album album)
        {
            this.context.Add(album);
        }

        public virtual async Task<IEnumerable<Album>> GetAlbums(AlbumQuery filter)
        {
            IQueryable<Album> query = this.context.Albums
                .Include(a => a.Photos)
                .Include(a => a.Author)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(filter.AuthorUserName))
                query = query.Where(a => a.Author.UserName == filter.AuthorUserName);
            if (filter.CategoryId.HasValue)
                query = query.Where(a => a.Photos.Any(p => p.PhotoCategories.Select(pc => pc.CategoryId).Any(id => id == filter.CategoryId)));
            if (filter.HasLocation.HasValue)
            {
                if (filter.HasLocation.Value)
                    query = query.Where(a => a.Photos.Any(p => p.LocLat.HasValue && p.LocLng.HasValue));
                else
                    query = query.Where(a => a.Photos.Any(p => !p.LocLat.HasValue || !p.LocLng.HasValue));
            }
            var albums = await query.ToListAsync();
            return albums;
        }

        public virtual async Task<Album> GetAsync(int id, bool includeRelated = true)
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

        public virtual void Delete(Album album)
        {
            this.context.Remove(album);
        }
    }
}