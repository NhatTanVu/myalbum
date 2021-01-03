using Moq;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class PhotoUploadService_Test3
    {
        [Fact]
        public void CopyPhoto()
        {
            // Arrange
            var photoStorage = new Mock<IPhotoStorage>();
            photoStorage.Setup(m => m.CopyPhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var uploadService = new PhotoUploadService(photoStorage.Object);
            // Act
            uploadService.CopyPhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            // Assert
            photoStorage.Verify(x => x.CopyPhoto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}