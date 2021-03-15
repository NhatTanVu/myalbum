using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAlbum.Services.Comment.API.Core
{
    public interface ICommentRepository
    {
        void Add(Models.Comment comment);
        Task<Models.Comment> GetAsync(int id, bool includeRelated = true);
        IEnumerable<Models.Comment> GetReplies(int id);
        IEnumerable<Models.Comment> GetSelfAndAncestors(int id);
        void DeleteAll(List<Models.Comment> comments);
        void Delete(Models.Comment comment);
    }
}