using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CommentRepository_Test3
    {
        private IEnumerable<Comment> SeedReplies(int seedCommentId, MyAlbumDbContext context)
        {
            string seedUserId = Guid.NewGuid().ToString();
            int seedPhotoId = new Random().Next(1, 100);
            Photo seedPhoto = new Photo()
            {
                Id = seedPhotoId
            };
            string expectedUserName = "test@gmail.com";
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = expectedUserName
            };
            int numOfReplies = new Random().Next(1, 20);
            List<Comment> seedReplies = new List<Comment>(numOfReplies);

            for (int i = 1; i <= numOfReplies; i++)
            {
                Comment seedReply = new Comment()
                {
                    Id = seedCommentId + i,
                    ParentId = (i == 0) ? (int?)null : seedCommentId,
                    Content = Guid.NewGuid().ToString(),
                    Photo = seedPhoto,
                    Author = seedUser
                };
                context.Comments.Add(seedReply);
                seedReplies.Add(seedReply);
            }
            context.SaveChanges();

            return seedReplies;
        }

        [Fact]
        public void GetReplies()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "GetReplies_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                int seedCommentId = new Random().Next(1, 100);
                IEnumerable<Comment> seedReplies = SeedReplies(seedCommentId, context);
                CommentRepository commentRepository = new CommentRepository(context);
                // Act
                var replies = commentRepository.GetReplies(seedCommentId);
                // Assert
                Assert.Equal(seedReplies, replies);
            }
        }
    }
}