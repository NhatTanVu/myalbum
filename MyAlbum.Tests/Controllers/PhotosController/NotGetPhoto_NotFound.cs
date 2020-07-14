using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MyAlbum.WebSPA.Controllers;
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
    public class PhotosController_Test5
    {
        private readonly IMapper _mapper;

        public PhotosController_Test5()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private int GetRandomIntExcept(List<int> excludedId)
        {
            int result = new Random().Next(1, 100);
            while (excludedId.Contains(result))
                result = new Random().Next(1, 100);
            return result;
        }

        [Fact]
        public async Task NotGetPhoto_NotFound()
        {
            // Arrange
            var seedIds = new List<int> { new Random().Next(1, 50), new Random().Next(51, 100) };
            var notFoundId = GetRandomIntExcept(seedIds);
            var mockPhotoRepository = new Mock<IPhotoRepository>();
            mockPhotoRepository.Setup(m => m.GetAsync(notFoundId, true)).ReturnsAsync((Photo)null);

            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPhotoUploadService = new Mock<IPhotoUploadService>();
            var mockHost = new Mock<IWebHostEnvironment>();
            mockHost.SetupGet(m => m.WebRootPath).Returns(string.Empty);
            var mockObjectDetectionService = new Mock<IObjectDetectionService>();
            PhotosController controller = new PhotosController(this._mapper, mockPhotoRepository.Object, 
                mockCategoryRepository.Object, mockUserRepository.Object, mockUnitOfWork.Object, 
                mockPhotoUploadService.Object, mockHost.Object, mockObjectDetectionService.Object);
            // Act
            var result = await controller.GetPhoto(notFoundId);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}