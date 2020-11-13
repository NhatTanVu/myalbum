using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class PhotoRepository_Test1
    {
        [Fact]
        public async Task AddPhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "PhotoRepository_AddPhoto_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                PhotoRepository photoRepository = new PhotoRepository(context);
                string seed = Guid.NewGuid().ToString();
                string seedUserId = Guid.NewGuid().ToString();
                int seedPhotoId = new Random().Next(1, 100);
                string expectedUserName = string.Format("test_{0}@gmail.com", seed);
                Photo originalPhoto = new Photo()
                {
                    Id = seedPhotoId,
                    Author = new User()
                    {
                        Id = seedUserId,
                        UserName = expectedUserName
                    }
                };
                // Act
                photoRepository.Add(originalPhoto);
                await unitOfWork.CompleteAsync();
                // Assert
                Assert.Equal(1, await context.Photos.CountAsync(c => true));
                var savedPhoto = await context.Photos.FirstAsync(c => true);
                Assert.NotEqual(0, savedPhoto.Id);
                Assert.Equal(originalPhoto.Name, savedPhoto.Name);
                Assert.Equal(originalPhoto.Author.Id, savedPhoto.Author.Id);
                Assert.Equal(originalPhoto.Author.UserName, savedPhoto.Author.UserName);
            }            
        }
    }
}