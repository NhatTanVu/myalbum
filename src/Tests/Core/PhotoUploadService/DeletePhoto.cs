using Moq;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class PhotoUploadService_Test2
    {
        [Fact]
        public void DeletePhoto()
        {
            // Arrange
            var photoStorage = new Mock<IPhotoStorage>();
            photoStorage.Setup(m => m.DeletePhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var uploadService = new PhotoUploadService(photoStorage.Object);
            // Act
            uploadService.DeletePhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            // Assert
            photoStorage.Verify(x => x.DeletePhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}