using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class PhotoRepository_Test6
    {
        private IFormFile GetFormFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var ms = new MemoryStream(File.ReadAllBytes(filePath));
            var formFile = new FormFile(ms, 0, ms.Length, filePath, filePath);

            return formFile;
        }

        [Theory]
        [InlineData("car.jpg", "GetImageDimensionsJPG_MyAlbumDatabase", 1920, 1536)]
        [InlineData("car.bmp", "GetImageDimensionsBMP_MyAlbumDatabase", 1920, 1536)]
        [InlineData("car.png", "GetImageDimensionsPNG_MyAlbumDatabase", 1920, 1536)]
        [InlineData("", "GetImageDimensionsEmpty_MyAlbumDatabase", 0, 0)]
        public async Task GetImageDimensions(string filePath, string databaseName, int width, int height)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                PhotoRepository photoRepository = new PhotoRepository(context);
                IFormFile file = null;
                if (!string.IsNullOrEmpty(filePath))
                    file = GetFormFile(filePath);
                
                // Act
                var result = await photoRepository.GetImageDimensions(file);
                // Assert
                Assert.Equal(width, result.Width);
                Assert.Equal(height, result.Height);
            }
        }
    }
}