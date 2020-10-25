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
    public class PhotosController_Test6
    {
        private readonly IMapper _mapper;

        public PhotosController_Test6()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private Mock<HttpContext> MockHttpContext(IEnumerable<CategoryResource> categories)
        {
            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.Setup(r => r.Request.Form).Returns(delegate()
            {
                var dict = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
                dict.Add("PhotoCategories", Newtonsoft.Json.JsonConvert.SerializeObject(categories));
                var nv = new Microsoft.AspNetCore.Http.FormCollection(dict);
                return nv;
            });

            return mockHttpContext;
        }

        [Fact]
        public async Task SavePhoto()
        {
            // Arrange
            int photoId = new Random().Next(1, 100);
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
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);
            var seedPhoto = new Photo()
            {
                Id = photoId,
                Name = "Photo " + photoId,
                FilePath = @"C:\Photo\File\Path\" + photoId,
                Author = new User() {
                    UserName = expectedUserName
                },
                LocLng = new Random().Next(1, 100),
                LocLat = new Random().Next(1, 100),
                CenterLng = new Random().Next(1, 100),
                CenterLat = new Random().Next(1, 100),
                MapZoom = new Random().Next(1, 100),                
            };
            mockPhotoRepository.Setup(m => m.GetAsync(photoId, true)).ReturnsAsync(seedPhoto);

            var mockCommentRepository = new Mock<ICommentRepository>();

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

            var mockUserRepository = new Mock<IUserRepository>();
            User expectedUser = new User() {
                Id = seed,
                UserName = expectedUserName
            };
            mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            PhotosController controller = new PhotosController(this._mapper, mockPhotoRepository.Object, mockCategoryRepository.Object,
                mockUserRepository.Object, mockCommentRepository.Object, mockUnitOfWork.Object, photoUploadService.Object, mockHost.Object, mockObjectDetectionService.Object);  
            PhotoResource originalResource = new PhotoResource()
            {
                Id = photoId,
                FileToUpload = new Mock<IFormFile>().Object,
                Name = seed + "_1",
                LocLng = seedPhoto.LocLng + 1,
                LocLat = seedPhoto.LocLat + 1,
                CenterLng = seedPhoto.CenterLng + 1,
                CenterLat = seedPhoto.CenterLat + 1,
                MapZoom = seedPhoto.MapZoom + 1,
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
            var mockHttpContext = MockHttpContext(originalResource.PhotoCategories);
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName, mockHttpContext);            
            controller.ControllerContext = controllerContext;
            // Act
            var result = await controller.SavePhoto(photoId, originalResource);
            originalResource.BoundingBoxFilePath = string.Format("{0}/{1}", controller.OutputFolderUrl, originalResource.FilePath);
            originalResource.FilePath = string.Format("{0}/{1}", controller.UploadFolderUrl, originalResource.FilePath);            
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PhotoResource>(((OkObjectResult)result).Value);
            PhotoResource returnedPhotoResource = (PhotoResource)((OkObjectResult)result).Value;
            Assert.True(returnedPhotoResource.Equals(originalResource));
        }
    }
}