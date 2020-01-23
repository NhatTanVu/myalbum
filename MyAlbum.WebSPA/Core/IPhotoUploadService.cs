using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Core
{
    public interface IPhotoUploadService
    {
        Task<string> UploadPhoto(IFormFile formFile, string uploadsFolderPath);
    }
}