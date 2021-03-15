using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(IFormFile formFile, string uploadsFolderPath);
        void DeletePhoto(string fileName, string uploadsFolderPath, string outputFolderPath);
        void CopyPhoto(string fileName, string uploadsFolderPath, string outputFolderPath);
    }
}