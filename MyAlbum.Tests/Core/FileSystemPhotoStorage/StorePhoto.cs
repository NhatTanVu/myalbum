using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class FileSystemPhotoStorage_Test1
    {
        private IFormFile GetFormFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var ms = new MemoryStream(File.ReadAllBytes(filePath));
            var formFile = new FormFile(ms, 0, ms.Length, filePath, filePath);

            return formFile;
        }

        [Fact]
        public async Task StorePhoto()
        {
            // Arrange
            string originalFilePath = Path.Combine(".", "car.jpg");
            string uploadsFolderPath = Path.Combine(".", "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            IFormFile file = GetFormFile(originalFilePath);
            var filePhotoStorage = new FileSystemPhotoStorage();
            // Act
            var result = await filePhotoStorage.StorePhoto(file, uploadsFolderPath);
            // Assert
            string expectedFilePath = Path.Combine(uploadsFolderPath, result);
            Assert.True(File.Exists(expectedFilePath));
            File.Delete(expectedFilePath);
        }
    }
}