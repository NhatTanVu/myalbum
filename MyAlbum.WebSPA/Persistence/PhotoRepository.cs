using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;

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
            {
                // TODO: Find a better implementation
                var photo = await context.Photos
                    .IncludeFilter(p => p.Author)
                    .IncludeFilter(p => p.Album)
                    .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null))
                        .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null)
                            .Select(c => c.Author))
                    .FirstOrDefaultAsync(p => p.Id == id);
                var photoWithCategories = await context.Photos
                    .Include(p => p.PhotoCategories)
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(v => v.Id == id);
                var replies = (from c1 in context.Comments
                               where (c1.Photo.Id == id && c1.ParentId == null)
                               join c2 in context.Comments on c1.Id equals c2.ParentId
                               into grpJoin
                               from r in grpJoin
                               group r by r.ParentId
                        into grp
                               select new
                               {
                                   ID = grp.Key,
                                   NumOfReplies = grp.Count()
                               }).ToList();

                foreach (var comment in photo.Comments)
                    if (replies.Any(r => r.ID == comment.Id))
                        comment.NumOfReplies = replies.First(r => r.ID == comment.Id).NumOfReplies;

                return photo;
            }
        }

        public async Task<IEnumerable<Photo>> GetPhotos(PhotoQuery filter)
        {
            IQueryable<Photo> query = this.context.Photos
                .Include(p => p.PhotoCategories)
                .AsQueryable();

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.PhotoCategories.Select(pc => pc.CategoryId).Any(id => id == filter.CategoryId));
            var photos = await query.ToListAsync();
            return photos;
        }
    }
}