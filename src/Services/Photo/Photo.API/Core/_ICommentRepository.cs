using System.Collections.Generic;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface ICommentRepository
    {
        void DeleteAll(List<Models.Comment> comments);
    }
}