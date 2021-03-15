//using System.Linq;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Z.EntityFramework.Plus;
using MyAlbum.Services.Comment.API.Core;
using Models = MyAlbum.Services.Comment.API.Core.Models;

namespace MyAlbum.Services.Comment.API.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyAlbumDbContext context;
        public PhotoRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<Models.Photo> GetAsync(int id/*, bool includeRelated = true*/)
        {
            //if (!includeRelated)
                return await context.Photos.FindAsync(id);
            //else
            //{
            //    // TODO: Find a better implementation
            //    var photo = await context.Photos
            //        .IncludeFilter(p => p.Author)
            //        .IncludeFilter(p => p.Album)
            //        .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null))
            //            .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null)
            //                .Select(c => c.Author))
            //        .FirstOrDefaultAsync(p => p.Id == id);

            //    if (photo == null)
            //        return null;

            //    var photoWithCategories = await context.Photos
            //        .Include(p => p.PhotoCategories)
            //            .ThenInclude(pc => pc.Category)
            //        .FirstOrDefaultAsync(v => v.Id == id);
            //    var replies = (from c1 in context.Comments
            //                   where (c1.Photo.Id == id && c1.ParentId == null)
            //                   join c2 in context.Comments on c1.Id equals c2.ParentId
            //                   into grpJoin
            //                   from r in grpJoin
            //                   group r by r.ParentId
            //            into grp
            //                   select new
            //                   {
            //                       ID = grp.Key,
            //                       NumOfReplies = grp.Count()
            //                   }).ToList();

            //    foreach (var comment in photo.Comments)
            //        if (replies.Any(r => r.ID == comment.Id))
            //            comment.NumOfReplies = replies.First(r => r.ID == comment.Id).NumOfReplies;

            //    return photo;
            //}
        }
    }
}