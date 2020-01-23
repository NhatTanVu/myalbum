using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IPhotoRepository
    {
        void Add(Photo photo);
        Task<IEnumerable<Photo>> GetPhotos();
    }
}