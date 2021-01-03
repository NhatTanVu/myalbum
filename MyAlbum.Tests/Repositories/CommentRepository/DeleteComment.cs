using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CommentRepository_Test9
    {
        private IEnumerable<Comment> SeedComments(int seedCommentId, MyAlbumDbContext context)
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
            int numOfReplies = new Random().Next(3, 5);
            List<Comment> seedReplies = new List<Comment>(numOfReplies);

            for (int i = 1; i <= numOfReplies; i++)
            {
                Comment seedReply = new Comment()
                {
                    Id = seedCommentId + i,
                    ParentId = null,
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
        public async Task DeleteComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentRepository_DeleteComment_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                int seedCommentId = new Random().Next(1, 100);
                List<Comment> seedComments = SeedComments(seedCommentId, context).ToList();
                CommentRepository commentRepository = new CommentRepository(context);
                Comment deletedComment = seedComments[0];
                // Assert #1
                Assert.Equal(seedComments.Count(), context.Comments.Count());
                // Act
                commentRepository.Delete(deletedComment);
                await unitOfWork.CompleteAsync();
                // Assert #2
                seedComments.Remove(deletedComment);
                Assert.True(seedComments.SequenceEqual(context.Comments));
            }
        }
    }
}