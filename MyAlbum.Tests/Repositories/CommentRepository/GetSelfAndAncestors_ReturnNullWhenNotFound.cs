using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CommentRepository_Test6
    {
        [Fact]
        public void GetSelfAndAncestors_ReturnNullWhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentRepository_GetSelfAndAncestors_ReturnNullWhenNotFound_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                int seedCommentId = new Random().Next(5, 100);
                IEnumerable<Comment> seedReplies = SeedSelfAndAncestors(seedCommentId, context);
                CommentRepository commentRepository = new CommentRepository(context);
                // Act
                var replies = commentRepository.GetSelfAndAncestors(seedCommentId - 1);
                // Assert
                Assert.Equal(0, replies.Count());
            }
        }

        private IEnumerable<Comment> SeedSelfAndAncestors(int seedCommentId, MyAlbumDbContext context)
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
            int numOfLevels = new Random().Next(1, 20);
            List<Comment> seedReplies = new List<Comment>(numOfLevels + 1);

            int? parentId = null;
            for (int i = numOfLevels; i >= 0; i--)
            {
                Comment seedReply = new Comment()
                {
                    Id = seedCommentId + i,
                    ParentId = parentId,
                    Content = Guid.NewGuid().ToString(),
                    Photo = seedPhoto,
                    Author = seedUser
                };
                context.Comments.Add(seedReply);
                seedReplies.Add(seedReply);
                parentId = seedReply.Id;
            }
            context.SaveChanges();
            seedReplies.Reverse();

            return seedReplies;
        }
    }
}