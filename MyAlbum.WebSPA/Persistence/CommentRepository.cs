using System.Threading.Tasks;
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

        public Task<Comment> GetAsync(int id, bool includeRelated = true)
        {
            throw new System.NotImplementedException();
        }
    }
}