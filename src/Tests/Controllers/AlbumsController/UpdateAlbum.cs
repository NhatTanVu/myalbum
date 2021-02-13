using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using MyAlbum.WebSPA.Controllers;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.WebSPA.Mapping;
using Xunit;

namespace MyAlbum.Tests.Controllers
{
    public class AlbumsController_Test5
    {
        private readonly IMapper _mapper;

        public AlbumsController_Test5()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }        

        [Fact]
        public async Task UpdateAlbum()
        {
            // Arrange
            int albumId = new Random().Next(1, 100);
            string seed = Guid.NewGuid().ToString();
            string expectedUserName = string.Format("test_{0}@gmail.com", Guid.NewGuid());
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);
            var mockAlbumRepository = new Mock<IAlbumRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            User expectedUser = new User() {
                Id = seed,
                UserName = expectedUserName
            };
            mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            AlbumsController controller = new AlbumsController(this._mapper, mockAlbumRepository.Object, mockUserRepository.Object, mockUnitOfWork.Object);
            controller.ControllerContext = controllerContext;
            var seedAlbum = new Album()
            {
                Id = albumId,
                Name = "Album " + albumId,
                Author = new User() {
                    UserName = expectedUserName
                },
            };
            mockAlbumRepository.Setup(m => m.GetAsync(albumId, true)).ReturnsAsync(seedAlbum);
            AlbumResource updateResource = new AlbumResource()
            {
                Id = albumId,
                Name = seed
            };
            // Act
            var result = await controller.UpdateAlbum(albumId, updateResource);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AlbumResource>(((OkObjectResult)result).Value);
            AlbumResource returnedResource = (AlbumResource)((OkObjectResult)result).Value;
            Assert.True(returnedResource.Equals(updateResource));            
        }
    }
}