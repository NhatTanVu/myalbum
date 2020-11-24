using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AlbumsController_Test4
    {
        private readonly IMapper _mapper;

        public AlbumsController_Test4()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Album> SeedAlbums()
        {
            List<Album> seedAlbums = new List<Album>();
            seedAlbums.Add(new Album(){
                Id = new Random().Next(1, 50),
                Name = Guid.NewGuid().ToString(),
            });
            seedAlbums.Add(new Album(){
                Id = new Random().Next(51, 100),
                Name = Guid.NewGuid().ToString(),
            });
            return seedAlbums;
        }

        [Fact]
        public async Task GetAlbums()
        {
            // Arrange
            var seedAlbums = SeedAlbums();
            var seedAlbumResources = this._mapper.Map<IEnumerable<Album>, IEnumerable<AlbumResource>>(seedAlbums);
            string seed = Guid.NewGuid().ToString();
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);
            var mockAlbumRepository = new Mock<IAlbumRepository>();
            mockAlbumRepository.Setup(m => m.GetAlbums(It.IsAny<AlbumQuery>())).ReturnsAsync(seedAlbums);
            var mockUserRepository = new Mock<IUserRepository>();
            User expectedUser = new User() {
                Id = seed,
                UserName = expectedUserName
            };
            mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            AlbumsController controller = new AlbumsController(this._mapper, mockAlbumRepository.Object, mockUserRepository.Object, mockUnitOfWork.Object);
            controller.ControllerContext = controllerContext;
            var filterResource = new AlbumQueryResource();
            // Act
            var albums = await controller.GetAlbums(filterResource);
            // Assert
            Assert.True(seedAlbumResources.SequenceEqual(albums));
        }        
    }
}