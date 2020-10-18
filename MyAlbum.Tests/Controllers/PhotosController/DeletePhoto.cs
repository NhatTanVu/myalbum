using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MyAlbum.WebSPA.Controllers;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.WebSPA.Mapping;
using MyAlbum.Core.Models;
using Xunit;
using MyAlbum.Core;
using Moq;
using Microsoft.AspNetCore.Hosting;
using MyAlbum.WebSPA.Core.ObjectDetection;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MyAlbum.Tests.Controllers
{
    public class PhotosController_Test7
    {
        private readonly IMapper _mapper;

        public PhotosController_Test7()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedPhotos(List<int> seedIds, string expectedUserName)
        {
            List<Photo> seedPhotos = new List<Photo>();

            foreach (var id in seedIds)
            {
                seedPhotos.Add(new Photo()
                {
                    Id = id,
                    Name = "Photo " + id,
                    FilePath = @"C:\Photo\File\Path\" + id,
                    Author = new User()
                    {
                        UserName = expectedUserName
                    },
                });
            }

            return seedPhotos;
        }

        [Fact]
        public async Task DeletePhoto()
        {
            // Arrange
            var seedIds = new List<int> { new Random().Next(1, 50), new Random().Next(51, 100) };
            string expectedUserName = string.Format("test_{0}@gmail.com", Guid.NewGuid());
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);
            var seedPhotos = SeedPhotos(seedIds, expectedUserName);
            var mockPhotoRepository = new Mock<IPhotoRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPhotoUploadService = new Mock<IPhotoUploadService>();
            var mockHost = new Mock<IWebHostEnvironment>();
            mockHost.SetupGet(m => m.WebRootPath).Returns(string.Empty);
            var mockObjectDetectionService = new Mock<IObjectDetectionService>();

            PhotosController controller = new PhotosController(this._mapper, mockPhotoRepository.Object,
                mockCategoryRepository.Object, mockUserRepository.Object, mockCommentRepository.Object, mockUnitOfWork.Object,
                mockPhotoUploadService.Object, mockHost.Object, mockObjectDetectionService.Object);
            controller.ControllerContext = controllerContext;
            int count = 0;
            foreach (var seedPhoto in seedPhotos)
            {
                var id = seedPhoto.Id;
                mockPhotoRepository.Setup(m => m.GetAsync(id, true)).ReturnsAsync(seedPhoto);
                mockPhotoUploadService.Setup(m => m.DeletePhoto(seedPhoto.FilePath, controller.UploadFolderPath, controller.OutputFolderPath));
                // Act
                var result = await controller.DeletePhoto(id);
                // Assert
                Assert.IsType<OkResult>(result);
                mockPhotoRepository.Verify(m => m.Delete(seedPhoto), Times.Once);
                mockCommentRepository.Verify(m => m.DeleteAll(It.IsAny<List<Comment>>()), Times.Exactly(++count));
                mockPhotoUploadService.Verify(m => m.DeletePhoto(seedPhoto.FilePath, controller.UploadFolderPath, controller.OutputFolderPath), Times.Once);
            }
        }
    }
}