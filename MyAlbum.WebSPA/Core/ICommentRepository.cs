using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        Task<Comment> GetAsync(int id, bool includeRelated = true);
        IEnumerable<Comment> GetReplies(int id);
        IEnumerable<Comment> GetSelfAndAncestors(int id);
        void DeleteAll(List<Comment> comments);
    }
}