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
    public class AlbumsController_Test1
    {
        private readonly IMapper _mapper;

        public AlbumsController_Test1()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task AddAlbum()
        {
            // Arrange
            string seed = Guid.NewGuid().ToString();
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);
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
            var originalResource = new AlbumResource()
            {
                Id = new Random().Next(1, 100),
                Name = seed,
                Author = new UserResource() {
                    UserName = expectedUserName
                }
            };
            // Act
            var result = await controller.CreateAlbum(originalResource);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AlbumResource>(((OkObjectResult)result).Value);
            AlbumResource returnedAlbumResource = (AlbumResource)((OkObjectResult)result).Value;
            Assert.True(returnedAlbumResource.Equals(originalResource));            
        }        
    }
}