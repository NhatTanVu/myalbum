using System;
using System.Collections.Generic;
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
    public class AlbumsController_Test3
    {
        private readonly IMapper _mapper;

        public AlbumsController_Test3()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Album> SeedAlbums(List<int> seedIds)
        {
            List<Album> seedAlbums = new List<Album>();

            foreach (var id in seedIds)
            {
                seedAlbums.Add(new Album()
                {
                    Id = id,
                    Name = "Album " + id
                });
            }

            return seedAlbums;
        }

        [Fact]
        public async Task GetPhoto()
        {
            // Arrange
            var seedIds = new List<int> { 
                new Random().Next(1, 50), 
                new Random().Next(51, 100), 
                new Random().Next(101, 150),
                new Random().Next(151, 200)
            };
            var seedAlbums = SeedAlbums(seedIds);
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
            foreach (var seedAlbum in seedAlbums)
            {
                var id = seedAlbum.Id;
                var seedAlbumResource = this._mapper.Map<Album, AlbumResource>(seedAlbum);
                mockAlbumRepository.Setup(m => m.GetAsync(id, true)).ReturnsAsync(seedAlbum);
                // Act
                var result = await controller.GetAlbum(id);
                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.IsType<AlbumResource>(((OkObjectResult)result).Value);
                AlbumResource returnedAlbumResource = (AlbumResource)((OkObjectResult)result).Value;
                Assert.True(returnedAlbumResource.Equals(seedAlbumResource));
            }
        }
    }
}