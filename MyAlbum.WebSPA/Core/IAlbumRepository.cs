using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IAlbumRepository
    {
        void Add(Album album);
        Task<Album> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Album>> GetAlbums(AlbumQuery filter);
        void Delete(Album album);
    }
}