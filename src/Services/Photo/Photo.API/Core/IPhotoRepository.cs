using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface IPhotoRepository
    {
        void Add(Models.Photo photo);
        Task<Models.Photo> GetAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Models.Photo>> GetPhotos(Models.PhotoQuery filter);
        Task<(int Height, int Width)> GetImageDimensions(IFormFile file);
        void Delete(Models.Photo photo);
    }
}