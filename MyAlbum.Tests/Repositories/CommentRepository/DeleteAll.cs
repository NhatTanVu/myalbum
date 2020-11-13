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
    public class CommentRepository_Test5
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
            int numOfReplies = new Random().Next(5, 20);
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
        public async Task DeleteAll()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "CommentRepository_DeleteAll_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                int seedCommentId = new Random().Next(1, 100);
                IEnumerable<Comment> seedReplies = SeedReplies(seedCommentId, context);
                CommentRepository commentRepository = new CommentRepository(context);
                // Assert #1
                Assert.Equal(seedReplies.Count(), context.Comments.Count());
                // Act
                commentRepository.DeleteAll(seedReplies.ToList());
                await unitOfWork.CompleteAsync();
                // Assert #2
                Assert.Equal(0, context.Comments.Count());
            }
        }
    }
}