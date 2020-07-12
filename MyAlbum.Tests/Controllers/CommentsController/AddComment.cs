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
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.SignalR;
using MyAlbum.WebSPA.Hubs;

namespace MyAlbum.Tests.Controllers
{
    public class CommentsController_Test1
    {
        private readonly IMapper _mapper;

        public CommentsController_Test1()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task AddComment()
        {
            // Arrange
            string seed = Guid.NewGuid().ToString();
            int seedPhotoId = new Random().Next(1, 100);

            string expectedUserName = string.Format("test_{0}@gmail.com", seed);
            ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);

            var mockUserRepository = new Mock<IUserRepository>();
            User expectedUser = new User()
            {
                Id = seed,
                UserName = expectedUserName
            };
            mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);

            var mockPhotoRepository = new Mock<IPhotoRepository>();
            mockPhotoRepository.Setup(m => m.GetAsync(seedPhotoId, true)).ReturnsAsync(new Photo()
            {
                Id = seedPhotoId
            });

            var mockHubClients = new Mock<IHubClients>();
            mockHubClients.SetupGet(m => m.All).Returns(new Mock<IClientProxy>().Object);
            var mockCommentHub = new Mock<IHubContext<CommentHub>>();
            mockCommentHub.SetupGet(m => m.Clients).Returns(mockHubClients.Object);

            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            CommentsController controller = new CommentsController(this._mapper, mockCommentHub.Object,
                mockCommentRepository.Object, mockUserRepository.Object, mockPhotoRepository.Object, mockUnitOfWork.Object);
            controller.ControllerContext = controllerContext;
            CommentResource originalResource = new CommentResource()
            {
                Id = new Random().Next(1, 100),
                Content = seed,
                PhotoId = seedPhotoId,
                Author = new UserResource()
                {
                    UserName = expectedUserName
                }
            };
            // Act
            var result = await controller.AddComment(originalResource);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<CommentResource>(((OkObjectResult)result).Value);
            CommentResource returnedPhotoResource = (CommentResource)((OkObjectResult)result).Value;
            Assert.True(returnedPhotoResource.Equals(originalResource));
        }
    }
}