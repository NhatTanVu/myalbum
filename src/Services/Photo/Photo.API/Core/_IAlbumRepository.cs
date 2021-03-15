using System.Threading.Tasks;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface IAlbumRepository
    {
        Task<Models.Album> GetAsync(int id, bool includeRelated = true);
    }
}