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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using MyAlbum.WebSPA.Core.ObjectDetection.YoloParser;
using System;

namespace MyAlbum.Tests.Controllers
{
    public class PhotosController_Test1
    {
        private readonly IMapper _mapper;

        public PhotosController_Test1()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task AddPhoto()
        {
            // Arrange
            string seed = Guid.NewGuid().ToString();
            string expectedFilePath = string.Format("1_{0}.jpg", seed);
            var photoUploadService = new Mock<IPhotoUploadService>();
            photoUploadService.Setup(m => m.UploadPhoto(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(expectedFilePath);

            int expectedWidth = new Random().Next(1, 5000);
            int expectedHeight = new Random().Next(1, 5000);
            var mockPhotoRepository = new Mock<IPhotoRepository>();
            mockPhotoRepository.Setup(m => m.GetImageDimensions(It.IsAny<IFormFile>()))
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

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            PhotosController controller = new PhotosController(this._mapper, mockPhotoRepository.Object, mockCategoryRepository.Object,
                mockUserRepository.Object, mockUnitOfWork.Object, photoUploadService.Object, mockHost.Object, mockObjectDetectionService.Object);
            controller.ControllerContext = controllerContext;
            PhotoResource originalResource = new PhotoResource()
            {
                Id = new Random().Next(1, 100),
                FileToUpload = new Mock<IFormFile>().Object,
                Name = seed,
                LocLng = new Random().Next(1, 100),
                LocLat = new Random().Next(1, 100),
                CenterLng = new Random().Next(1, 100),
                CenterLat = new Random().Next(1, 100),
                MapZoom = new Random().Next(1, 100),
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
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PhotoResource>(((OkObjectResult)result).Value);
            PhotoResource returnedPhotoResource = (PhotoResource)((OkObjectResult)result).Value;
            Assert.True(returnedPhotoResource.Equals(originalResource));
        }
    }
}