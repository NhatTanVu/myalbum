using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IPhotoRepository
    {
        void Add(Photo photo);
        Task<Photo> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Photo>> GetPhotos(PhotoQuery filter);
        Task<(int Height, int Width)> GetImageDimensions(IFormFile file);
        void Delete(Photo photo);
    }
}