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
    public class PhotosController_CreatePhotoShould
    {
        private readonly IMapper _mapper;

        public PhotosController_CreatePhotoShould()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedMockDb_GetPhotos_ReturnPhotos(DbContextOptions<MyAlbumDbContext> options)
        {
            List<Photo> seededPhotos = new List<Photo>();
            seededPhotos.Add(new Photo(){
                Id = 1,
                Name = "Photo 1",
                FilePath = @"C:\Photo\File\Path\1"
            });
            seededPhotos.Add(new Photo(){
                Id = 2,
                Name = "Photo 2",
                FilePath = @"C:\Photo\File\Path\2"
            });
            using (var context = new MyAlbumDbContext(options))
            {
                foreach(Photo photo in seededPhotos)
                {
                    context.Photos.Add(photo);
                    context.SaveChanges();
                }
            }
            return seededPhotos;
        }

        [Fact]
        public async Task CreatePhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "PhotosController_CreatePhotoShould_MyAlbumDatabase")
                .Options;
            var seededPhotos = SeedMockDb_GetPhotos_ReturnPhotos(options);
            var seededPhotoResources = this._mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(seededPhotos);
            using (var context = new MyAlbumDbContext(options))
            {
                var photoRepository = new PhotoRepository(context);
                var categoryRepository = new CategoryRepository(context);
                var userRepository = new UserRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var photoUploadService = new PhotoUploadService(new FileSystemPhotoStorage());
                var mockHost = new Mock<IWebHostEnvironment>();
                mockHost.SetupGet(m => m.WebRootPath).Returns(string.Empty);
                var mockObjectDetectionService = new Mock<IObjectDetectionService>();
                PhotosController controller = new PhotosController(this._mapper, photoRepository, categoryRepository, userRepository, unitOfWork, photoUploadService, mockHost.Object, mockObjectDetectionService.Object);
                var filterResource = new PhotoQueryResource();
                // Act
                var photos = await controller.GetPhotos(filterResource);
                // Assert
                Assert.True(seededPhotoResources.SequenceEqual(photos));
            }
        }
    }
}