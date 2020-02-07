using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IPhotoRepository
    {
        void Add(Photo photo);
        Task<Photo> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Photo>> GetPhotos(PhotoQuery filter);
    }
}