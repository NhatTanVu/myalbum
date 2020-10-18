using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(IFormFile formFile, string uploadsFolderPath);
        void DeletePhoto(string filePath, string uploadsFolderPath, string outputFolderPath);
    }
}