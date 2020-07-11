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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using MyAlbum.WebSPA.Core.ObjectDetection.YoloParser;
using System.Security.Claims;
using System;

namespace MyAlbum.Tests.Controllers
{
    public class ShouldAddPhoto
    {
        private readonly IMapper _mapper;

        public ShouldAddPhoto()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedPhotos(DbContextOptions<MyAlbumDbContext> options)
        {
            List<Photo> seedPhotos = new List<Photo>();
            seedPhotos.Add(new Photo()
            {
                Id = 1,
                Name = "Photo 1",
                FilePath = @"C:\Photo\File\Path\1"
            });
            seedPhotos.Add(new Photo()
            {
                Id = 2,
                Name = "Photo 2",
                FilePath = @"C:\Photo\File\Path\2"
            });
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
        public async Task AddPhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AddPhoto_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                string seed = Guid.NewGuid().ToString();
                string expectedFilePath = string.Format("1_{0}.jpg", seed);
                var photoUploadService = new Mock<IPhotoUploadService>();
                photoUploadService.Setup(m => m.UploadPhoto(It.IsAny<IFormFile>(), It.IsAny<string>()))
                    .ReturnsAsync(expectedFilePath);

                int expectedWidth = new Random().Next();
                int expectedHeight = new Random().Next();
                var photoRepository = new Mock<PhotoRepository>(context);
                photoRepository.CallBase = true;
                photoRepository.Setup(m => m.GetImageDimensions(It.IsAny<IFormFile>()))
                    .ReturnsAsync((expectedHeight, expectedWidth));

                var mockHost = new Mock<IWebHostEnvironment>();
                string expectedWebRootPath = seed;
                mockHost.SetupGet(m => m.WebRootPath).Returns(expectedWebRootPath);

                IDictionary<string, IList<YoloBoundingBox>> expectedObjectsDict = new Dictionary<string, IList<YoloBoundingBox>>();
                string expectedLabel = string.Format("bird_{0}", seed);
                YoloBoundingBox expectedBoundingBox = new YoloBoundingBox() { Label = expectedLabel };
                string expectedKey = Path.Combine(expectedWebRootPath, "uploads", expectedFilePath);
                expectedObjectsDict.Add(expectedKey, new List<YoloBoundingBox>() { expectedBoundingBox });
                var mockObjectDetectionService = new Mock<IObjectDetectionService>();
                mockObjectDetectionService.Setup(m => m.DetectObjectsFromImages(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(expectedObjectsDict);

                var mockCategoryRepository = new Mock<ICategoryRepository>();
                Category expectedCategory = new Category() {
                    Name = expectedLabel
                };
                mockCategoryRepository.Setup(m => m.GetByNames(new List<string>() { expectedLabel }))
                    .Returns(new List<Category>() { expectedCategory });

                string expectedUserName = string.Format("test_{0}@gmail.com", seed);
                ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);

                var mockUserRepository = new Mock<IUserRepository>();
                User expectedUser = new User() {
                    Id = seed,
                    UserName = expectedUserName
                };
                mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);

                UnitOfWork unitOfWork = new UnitOfWork(context);

                PhotosController controller = new PhotosController(this._mapper, photoRepository.Object, mockCategoryRepository.Object,
                    mockUserRepository.Object, unitOfWork, photoUploadService.Object, mockHost.Object, mockObjectDetectionService.Object);
                controller.ControllerContext = controllerContext;
                PhotoResource originalResource = new PhotoResource()
                {
                    Id = new Random().Next(),
                    FileToUpload = new Mock<IFormFile>().Object,
                    Name = seed,
                    LocLng = new Random().Next(),
                    LocLat = new Random().Next(),
                    CenterLng = new Random().Next(),
                    CenterLat = new Random().Next(),
                    MapZoom = new Random().Next(),
                    FilePath = expectedFilePath,
                    Width = expectedWidth,
                    Height = expectedHeight,
                    PhotoCategories = new List<CategoryResource>() {
                        new CategoryResource() {
                            Name = expectedLabel
                        }
                    },
                    Author = new UserResource() {
                        UserName = expectedUserName
                    }
                };
                // Act
                var result = await controller.CreatePhoto(originalResource);
                // Assert #1
                Assert.IsType<OkObjectResult>(result);
                Assert.IsType<PhotoResource>(((OkObjectResult)result).Value);
                PhotoResource returnedPhotoResource = (PhotoResource)((OkObjectResult)result).Value;
                Assert.True(returnedPhotoResource.Equals(originalResource));
                // Assert #2 - PhotoRepository.Add()
                // Assert.Equal(context.Photos.Count(), 1);
                // Photo returnedPhoto = context.Photos.First(p => p.Name == originalResource.Name);
                // var originalPhoto = this._mapper.Map<PhotoResource, Photo>(originalResource);
                // Assert.True(returnedPhoto.Equals(originalPhoto));
                // Assert.Equal(returnedPhoto.PhotoCategories.Count, 1);
                // Assert.Equal(returnedPhoto.PhotoCategories.ElementAt(0).Category.Name, expectedLabel);
                // Assert.Equal(returnedPhoto.Author.Id, seed);
                // Assert.Equal(returnedPhoto.Author.UserName, expectedUserName);
            }
        }
    }
}