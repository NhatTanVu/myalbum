using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Services.Photo.API.Core;
using Models = MyAlbum.Services.Photo.API.Core.Models;

namespace MyAlbum.Services.Photo.API.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyAlbumDbContext context;
        public CommentRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public void DeleteAll(List<Models.Comment> comments)
        {
            List<int> deletedIds = new List<int>();
            foreach (var comment in comments)
            {
                deletedIds.AddRange(GetDeletedIds(comment));
            }
            var deletedComments = this.context.Comments.Where(c => deletedIds.Contains(c.Id));

            this.context.RemoveRange(deletedComments);
        }

        private IEnumerable<Models.Comment> GetReplies(int id)
        {
            IEnumerable<Models.Comment> replies = context.Comments
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

        private IEnumerable<int> GetDeletedIds(Models.Comment comment)
        {
            List<int> deletedIds = new List<int>();
            var replies = GetReplies(comment.Id);
            foreach (Models.Comment reply in replies)
            {
                deletedIds.AddRange(GetDeletedIds(reply));
            }
            deletedIds.Add(comment.Id);

            return deletedIds;
        }
    }
}