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
using Microsoft.AspNetCore.SignalR;
using MyAlbum.WebSPA.Hubs;

namespace MyAlbum.Tests.Controllers
{
    public class ShouldAddComment
    {
        private readonly IMapper _mapper;

        public ShouldAddComment()
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
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AddComment_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                string seed = Guid.NewGuid().ToString();
                int seedPhotoId = new Random().Next();

                string expectedUserName = string.Format("test_{0}@gmail.com", seed);
                ControllerContext controllerContext = Utilities.SetupCurrentUserForController(expectedUserName);

                var mockUserRepository = new Mock<IUserRepository>();
                User expectedUser = new User() {
                    Id = seed,
                    UserName = expectedUserName
                };
                mockUserRepository.Setup(m => m.GetOrAdd(It.IsAny<User>())).Returns(expectedUser);

                var mockPhotoRepository = new Mock<IPhotoRepository>();
                mockPhotoRepository.Setup(m => m.GetAsync(seedPhotoId, true)).ReturnsAsync(new Photo() {
                    Id = seedPhotoId
                });

                var mockHubClients = new Mock<IHubClients>();
                mockHubClients.SetupGet(m => m.All).Returns(new Mock<IClientProxy>().Object);
                var mockCommentHub = new Mock<IHubContext<CommentHub>>();
                mockCommentHub.SetupGet(m => m.Clients).Returns(mockHubClients.Object);

                CommentRepository commentRepository = new CommentRepository(context);
                UnitOfWork unitOfWork = new UnitOfWork(context);

                CommentsController controller = new CommentsController(this._mapper, mockCommentHub.Object, 
                    commentRepository, mockUserRepository.Object, mockPhotoRepository.Object, unitOfWork);
                controller.ControllerContext = controllerContext;
                CommentResource originalResource = new CommentResource()
                {
                    Id = new Random().Next(),
                    Content = seed,
                    PhotoId = seedPhotoId,
                    Author = new UserResource() {
                        UserName = expectedUserName
                    }
                };
                // Act
                var result = await controller.AddComment(originalResource);
                // Assert #1
                Assert.IsType<OkObjectResult>(result);
                Assert.IsType<CommentResource>(((OkObjectResult)result).Value);
                CommentResource returnedPhotoResource = (CommentResource)((OkObjectResult)result).Value;
                Assert.True(returnedPhotoResource.Equals(originalResource));
                // Assert #2 - CommentRepository.Add()
            }
        }
    }
}