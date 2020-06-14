using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using MyAlbum.WebSPA.Controllers.Resources;

namespace MyAlbum.Persistence
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyAlbumDbContext context;
        public CommentRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public void Add(Comment comment)
        {
            this.context.Add(comment);
        }

        public async Task<Comment> GetAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await this.context.Comments.FindAsync(id);
            else
                return await context.Comments
                    .Include(c => c.Photo)
                    .Include(c => c.Author)
                    .Include(c => c.Parent)
                    .Include(c => c.Replies)
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetRepliesAsync(int id)
        {
            IEnumerable<Comment> replies = await context.Comments
                    .Where(c => c.ParentId == id)
                    .Include(c => c.Author)
                    .Include(c => c.Photo)
                    .ToListAsync();

            var tempReplies = (from c1 in context.Comments
                               where (c1.ParentId == id)
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

            foreach (var reply in replies)
                if (tempReplies.Any(r => r.ID == reply.Id))
                    reply.NumOfReplies = tempReplies.First(r => r.ID == reply.Id).NumOfReplies;
            return replies;
        }
    }
}