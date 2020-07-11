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
using System;
using Microsoft.AspNetCore.Mvc;

namespace MyAlbum.Tests.Controllers
{
    public class ShouldGetPhoto
    {
        private readonly IMapper _mapper;

        public ShouldGetPhoto()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedPhotos(List<int> seedIds, DbContextOptions<MyAlbumDbContext> options)
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

            using (var context = new MyAlbumDbContext(options))
            {
                foreach (Photo photo in seedPhotos)
                {
                    context.Photos.Add(photo);
                    context.SaveChanges();
                }
            }
            return seedPhotos;
        }

        [Fact]
        public async Task GetPhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "GetPhoto_MyAlbumDatabase")
                .Options;
            var seedIds = new List<int> { new Random().Next(1, 100), new Random().Next(1, 100) };
            var seedPhotos = SeedPhotos(seedIds, options);
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
                foreach (var seedPhoto in seedPhotos)
                {
                    var id = seedPhoto.Id;
                    var seedPhotoResource = this._mapper.Map<Photo, PhotoResource>(seedPhoto);
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
}