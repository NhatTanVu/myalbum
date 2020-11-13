using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class PhotoUploadService_Test1
    {
        [Fact]
        public async Task UploadPhoto()
        {
            // Arrange
            string seed = Guid.NewGuid().ToString();
            string expectedFilePath = string.Format("1_{0}.jpg", seed);
            var photoStorage = new Mock<IPhotoStorage>();
            photoStorage.Setup(m => m.StorePhoto(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(expectedFilePath);
            var uploadService = new PhotoUploadService(photoStorage.Object);
            // Act
            var result = await uploadService.UploadPhoto(It.IsAny<IFormFile>(), It.IsAny<string>());
            // Assert
            Assert.Equal(expectedFilePath, result);
        }
    }
}