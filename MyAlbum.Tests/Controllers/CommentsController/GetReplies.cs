using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAlbum.WebSPA.Controllers;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.WebSPA.Mapping;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;
using Moq;
using System;
using Microsoft.AspNetCore.SignalR;
using MyAlbum.WebSPA.Hubs;
using MyAlbum.Core;

namespace MyAlbum.Tests.Controllers
{
    public class CommentsController_Test2
    {
        private readonly IMapper _mapper;

        public CommentsController_Test2()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Comment> SeedReplies(int photoId, int commentId)
        {
            Photo photo = new Photo() {
                Id = photoId
            };

            List<Comment> seedReplies = new List<Comment>();
            seedReplies.Add(new Comment()
            {
                Id = new Random().Next(1, 100),
                Content = Guid.NewGuid().ToString(),
                ParentId = commentId,
                Photo = photo
            });
            seedReplies.Add(new Comment()
            {
                Id = new Random().Next(1, 100),
                Content = Guid.NewGuid().ToString(),
                ParentId = commentId,
                Photo = photo
            });
            return seedReplies;
        }

        [Fact]
        public void GetReplies()
        {
            // Arrange
            int photoId = new Random().Next(1, 100);
            int commentId = new Random().Next(1, 100);
            var seedReplies = SeedReplies(photoId, commentId);
            var seedReplyResources = this._mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResource>>(seedReplies);            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPhotoRepository = new Mock<IPhotoRepository>();
            var mockCommentHub = new Mock<IHubContext<CommentHub>>();
            
            var mockCommentRepository = new Mock<ICommentRepository>();
            mockCommentRepository.Setup(m => m.GetReplies(commentId)).Returns(seedReplies);

            var unitOfWork = new Mock<IUnitOfWork>();

            CommentsController controller = new CommentsController(this._mapper, mockCommentHub.Object, 
                mockCommentRepository.Object, mockUserRepository.Object, mockPhotoRepository.Object, unitOfWork.Object);
            // Act
            var replies = controller.GetReplies(commentId);
            // Assert
            Assert.True(seedReplyResources.SequenceEqual(replies));
        }
    }
}