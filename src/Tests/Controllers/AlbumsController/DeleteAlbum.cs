using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using MyAlbum.WebSPA.Controllers;
using MyAlbum.WebSPA.Mapping;
using Xunit;

namespace MyAlbum.Tests.Controllers
{
    public class AlbumsController_Test2
    {
        private readonly IMapper _mapper;
        
        public AlbumsController_Test2()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Album> SeedAlbums(List<int> seedIds, string expectedUserName)
        {
            List<Album> seedAlbums = new List<Album>();

            foreach (var id in seedIds)
            {
                seedAlbums.Add(new Album()
                {
                    Id = id,
                    Name = "Photo " + id,
                    Author = new User()
                    {
                        UserName = expectedUserName
                    },
                });
            }

            return seedAlbums;
        }

        [Fact]
        public async Task DeleteAlbum()
        {
            // Arrange
            var seedIds = new List<int> { new Random().Next(1, 50), new Random().Next(51, 100) };
            string expectedUserName = string.Format("test_{0}@gmail.com", Guid.NewGuid());
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);
            var seedAlbums = SeedAlbums(seedIds, expectedUserName);
            string seed = Guid.NewGuid().ToString();
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
                mockAlbumRepository.Setup(m => m.GetAsync(id, true)).ReturnsAsync(seedAlbum);
                // Act
                var result = await controller.DeleteAlbum(id);
                // Assert
                Assert.IsType<OkResult>(result);
                mockAlbumRepository.Verify(m => m.Delete(seedAlbum), Times.Once);
            }
        }
    }
}