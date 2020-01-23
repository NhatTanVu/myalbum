using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Core
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        public async Task<string> StorePhoto(string uploadsFolderPath, IFormFile formFile)
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
    }
}