using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        Task<Comment> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Comment>> GetRepliesAsync(int id);
    }
}