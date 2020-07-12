using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAlbum.WebSPA.Controllers;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.WebSPA.Mapping;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;
using MyAlbum.Core;
using Moq;
using Microsoft.AspNetCore.Hosting;
using MyAlbum.WebSPA.Core.ObjectDetection;

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
                Id = 1,
                Name = "Photo 1",
                FilePath = @"C:\Photo\File\Path\1"
            });
            seedPhotos.Add(new Photo(){
                Id = 2,
                Name = "Photo 2",
                FilePath = @"C:\Photo\File\Path\2"
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