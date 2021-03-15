using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAlbum.Services.Album.API.Core
{
    public interface IAlbumRepository
    {
        void Add(Models.Album album);
        Task<Models.Album> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Models.Album>> GetAlbums(Models.AlbumQuery filter);
        void Delete(Models.Album album);
    }
}