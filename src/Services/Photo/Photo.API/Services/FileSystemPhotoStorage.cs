using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyAlbum.Services.Photo.API.Core;

namespace MyAlbum.Services.Photo.API.Services
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        public async Task<string> StorePhoto(IFormFile formFile, string uploadsFolderPath)
        {
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return fileName;
        }

        public void DeletePhoto(string fileName, string uploadsFolderPath, string outputFolderPath)
        {
            System.GC.Collect(); 
            System.GC.WaitForPendingFinalizers();            
            var uploadFilePath = Path.Combine(uploadsFolderPath, fileName);
            var outputFilePath = Path.Combine(outputFolderPath, fileName);
            File.Delete(uploadFilePath);
            File.Delete(outputFilePath);
        }

        public void CopyPhoto(string fileName, string uploadsFolderPath, string outputFolderPath)
        {
            var uploadFilePath = Path.Combine(uploadsFolderPath, fileName);
            var outputFilePath = Path.Combine(outputFolderPath, fileName);
            File.Copy(uploadFilePath, outputFilePath);
        }
    }
}