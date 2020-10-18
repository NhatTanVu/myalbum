using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Core
{
    public class PhotoUploadService : IPhotoUploadService
    {
        private readonly IPhotoStorage photoStorage;
        public PhotoUploadService(IPhotoStorage photoStorage)
        {
            this.photoStorage = photoStorage;
        }
        public async Task<string> UploadPhoto(IFormFile formFile, string uploadsFolderPath)
        {
            return await this.photoStorage.StorePhoto(formFile, uploadsFolderPath);
        }

        public void DeletePhoto(string filePath, string uploadsFolderPath, string outputFolderPath)
        {
            this.photoStorage.DeletePhoto(filePath, uploadsFolderPath, outputFolderPath);
        }
    }
}