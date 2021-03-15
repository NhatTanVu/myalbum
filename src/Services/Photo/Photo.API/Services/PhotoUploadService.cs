using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyAlbum.Services.Photo.API.Core;

namespace MyAlbum.Services.Photo.API.Services
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

        public void DeletePhoto(string fileName, string uploadsFolderPath, string outputFolderPath)
        {
            this.photoStorage.DeletePhoto(fileName, uploadsFolderPath, outputFolderPath);
        }

        public void CopyPhoto(string fileName, string uploadsFolderPath, string outputFolderPath)
        {
            this.photoStorage.CopyPhoto(fileName, uploadsFolderPath, outputFolderPath);
        }
    }
}