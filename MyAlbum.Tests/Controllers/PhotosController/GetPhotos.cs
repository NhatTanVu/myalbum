using System.Collections.Generic;
using System.Linq;
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

namespace MyAlbum.Tests.Controllers
{
    public class PhotosController_Test3
    {
        private readonly IMapper _mapper;

        public PhotosController_Test3()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedPhotos()
        {
            List<Photo> seedPhotos = new List<Photo>();
            seedPhotos.Add(new Photo(){
                Id = new Random().Next(1, 100),
                Name = Guid.NewGuid().ToString(),
                FilePath = @"C:\Photo\File\Path\" + new Random().Next(1, 100)
            });
            seedPhotos.Add(new Photo(){
                Id = new Random().Next(1, 100),
                Name = Guid.NewGuid().ToString(),
                FilePath = @"C:\Photo\File\Path\" + new Random().Next(1, 100)
            });
            return seedPhotos;
        }

        [Fact]
        public async Task GetPhotos()
        {
            // Arrange
            var seedPhotos = SeedPhotos();
            var seedPhotoResources = this._mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(seedPhotos);
            var filterResource = new PhotoQueryResource();
            var mockPhotoRepository = new Mock<IPhotoRepository>();
            mockPhotoRepository.Setup(m => m.GetPhotos(It.IsAny<PhotoQuery>())).ReturnsAsync(seedPhotos);
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPhotoUploadService = new Mock<IPhotoUploadService>();
            var mockHost = new Mock<IWebHostEnvironment>();
            mockHost.SetupGet(m => m.WebRootPath).Returns(string.Empty);
            var mockObjectDetectionService = new Mock<IObjectDetectionService>();

            PhotosController controller = new PhotosController(this._mapper, mockPhotoRepository.Object, 
                mockCategoryRepository.Object, mockUserRepository.Object, 
                mockUnitOfWork.Object, mockPhotoUploadService.Object, 
                mockHost.Object, mockObjectDetectionService.Object);
            // Act
            var photos = await controller.GetPhotos(filterResource);
            // Assert
            Assert.True(seedPhotoResources.SequenceEqual(photos));
        }
    }
}