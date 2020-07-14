using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CommentRepository_Test1
    {
        [Fact]
        public async Task AddComment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AddComment_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                CommentRepository commentRepository = new CommentRepository(context);
                string seed = Guid.NewGuid().ToString();
                string seedUserId = Guid.NewGuid().ToString();
                int seedPhotoId = new Random().Next(1, 100);
                string expectedUserName = string.Format("test_{0}@gmail.com", seed);
                Comment originalComment = new Comment()
                {
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
                // Act
                commentRepository.Add(originalComment);
                await unitOfWork.CompleteAsync();
                // Assert
                Assert.Equal(1, await context.Comments.CountAsync(c => true));
                var savedComment = await context.Comments.FirstAsync(c => true);
                Assert.NotEqual(0, savedComment.Id);
                Assert.Equal(originalComment.Content, savedComment.Content);
                Assert.Equal(originalComment.Photo.Id, savedComment.Photo.Id);
                Assert.Equal(originalComment.Author.Id, savedComment.Author.Id);
                Assert.Equal(originalComment.Author.UserName, savedComment.Author.UserName);
            }
        }
    }
}