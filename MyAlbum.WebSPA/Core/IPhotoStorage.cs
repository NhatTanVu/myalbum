using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile formFile);
    }
}