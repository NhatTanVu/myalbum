using System.Threading.Tasks;

namespace MyAlbum.Services.Comment.API.Core
{
    public interface IPhotoRepository
    {
        Task<Models.Photo> GetAsync(int id/*, bool includeRelated = true*/);
    }
}