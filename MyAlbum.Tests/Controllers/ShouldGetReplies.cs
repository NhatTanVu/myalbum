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
    public class ShouldGetReplies
    {
        private readonly IMapper _mapper;

        public ShouldGetReplies()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Comment> SeedReplies(int photoId, int commentId, DbContextOptions<MyAlbumDbContext> options)
        {
            Photo photo = new Photo() {
                Id = photoId
            };

            List<Comment> seedReplies = new List<Comment>();
            seedReplies.Add(new Comment()
            {
                Id = new Random().Next(),
                Content = Guid.NewGuid().ToString(),
                ParentId = commentId,
                Photo = photo
            });
            seedReplies.Add(new Comment()
            {
                Id = new Random().Next(),
                Content = Guid.NewGuid().ToString(),
                ParentId = commentId,
                Photo = photo
            });
            using (var context = new MyAlbumDbContext(options))
            {
                foreach (Comment comment in seedReplies)
                {
                    context.Comments.Add(comment);
                    context.SaveChanges();
                }
            }
            return seedReplies;
        }

        [Fact]
        public void GetReplies()
        {
            // Arrange
            int photoId = new Random().Next();
            int commentId = new Random().Next();
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "GetReplies_MyAlbumDatabase")
                .Options;
            var seedReplies = SeedReplies(photoId, commentId, options);
            var seedReplyResources = this._mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResource>>(seedReplies);            
            using (var context = new MyAlbumDbContext(options))
            {
                var userRepository = new UserRepository(context);
                var photoRepository = new PhotoRepository(context);
                var mockCommentHub = new Mock<IHubContext<CommentHub>>();
                CommentRepository commentRepository = new CommentRepository(context);
                UnitOfWork unitOfWork = new UnitOfWork(context);

                CommentsController controller = new CommentsController(this._mapper, mockCommentHub.Object, 
                    commentRepository, userRepository, photoRepository, unitOfWork);
                // Act
                var replies = controller.GetReplies(commentId);
                // Assert
                Assert.True(seedReplyResources.SequenceEqual(replies));
            }
        }
    }
}