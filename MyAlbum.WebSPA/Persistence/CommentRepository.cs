using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core;
using MyAlbum.Core.Models;

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

        public IEnumerable<Comment> GetReplies(int id)
        {
            IEnumerable<Comment> replies = context.Comments
                    .Where(c => c.ParentId == id)
                    .Include(c => c.Author)
                    .Include(c => c.Photo)
                    .ToList();

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

        public IEnumerable<Comment> GetSelfAndAncestors(int id)
        {
            List<Comment> result = new List<Comment>();

            var comment = this.context.Comments.Find(id);
            if (comment != null)
                result.Add(comment);

            while (comment != null && comment.ParentId != null)
            {
                int parentId = comment.ParentId.Value;
                comment = this.context.Comments.Find(parentId);
                comment.Replies = (ICollection<Comment>)this.GetReplies(parentId);
                comment.NumOfReplies = comment.Replies.Count;
                result.Add(comment);
            }

            return result;
        }

        private IEnumerable<int> GetDeletedIds(Comment comment)
        {
            List<int> deletedIds = new List<int>();
            var replies = GetReplies(comment.Id);
            foreach (Comment reply in replies)
            {
                deletedIds.AddRange(GetDeletedIds(reply));
            }
            deletedIds.Add(comment.Id);

            return deletedIds;
        }

        public void DeleteAll(List<Comment> comments)
        {
            List<int> deletedIds = new List<int>();
            foreach(var comment in comments)
            {
                deletedIds.AddRange(GetDeletedIds(comment));
            }
            var deletedComments = this.context.Comments.Where(c => deletedIds.Contains(c.Id));

            this.context.RemoveRange(deletedComments);
        }
    }
}