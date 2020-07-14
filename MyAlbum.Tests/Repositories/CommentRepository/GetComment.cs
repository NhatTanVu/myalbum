using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CommentRepository_Test2
    {
        private Comment SeedComment(MyAlbumDbContext context)
        {
            int seedCommentId = new Random().Next(1, 100);
            string seed = Guid.NewGuid().ToString();
            string seedUserId = Guid.NewGuid().ToString();
            int seedPhotoId = new Random().Next(1, 100);
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);            
            Comment seedComment = new Comment()
            {
                Id = seedCommentId,
                Content = seed,
                Photo = new Photo() {
                    Id = seedPhotoId
                },
                Author = new User()
                {
                    Id = seedUserId,
                    UserName = expectedUserName
                }
            };
            context.Comments.Add(seedComment);
            context.SaveChanges();
            return seedComment;
        }

        [Fact]
        public async Task GetComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "GetComment_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                Comment seedComment = SeedComment(context);
                CommentRepository commentRepository = new CommentRepository(context);
                // Act
                Comment result = await commentRepository.GetAsync(seedComment.Id);
                // Assert
                Assert.Equal(seedComment, result);
            }    
        }
    }
}