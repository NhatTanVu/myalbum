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
    public class PhotosController_Test2
    {
        private readonly IMapper _mapper;

        public PhotosController_Test2()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedPhotos(List<int> seedIds)
        {
            List<Photo> seedPhotos = new List<Photo>();

            foreach (var id in seedIds)
            {
                seedPhotos.Add(new Photo()
                {
                    Id = id,
                    Name = "Photo " + id,
                    FilePath = @"C:\Photo\File\Path\" + id
                });
            }

            return seedPhotos;
        }

        [Fact]
        public async Task GetPhoto()
        {
            // Arrange
            var seedIds = new List<int> { new Random().Next(1, 100), new Random().Next(1, 100) };
            var seedPhotos = SeedPhotos(seedIds);
            var mockPhotoRepository = new Mock<IPhotoRepository>();
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
            foreach (var seedPhoto in seedPhotos)
            {
                var id = seedPhoto.Id;
                var seedPhotoResource = this._mapper.Map<Photo, PhotoResource>(seedPhoto);
                mockPhotoRepository.Setup(m => m.GetAsync(id, true)).ReturnsAsync(seedPhoto);
                // Act
                var result = await controller.GetPhoto(id);
                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.IsType<PhotoResource>(((OkObjectResult)result).Value);
                PhotoResource returnedPhotoResource = (PhotoResource)((OkObjectResult)result).Value;
                Assert.True(returnedPhotoResource.Equals(seedPhotoResource));
            }
        }
    }
}