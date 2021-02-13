using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class PhotoRepository_Test2
    {
        private Photo SeedPhoto(MyAlbumDbContext context)
        {
            int seedPhotoId = new Random().Next(1, 100);
            string seed = Guid.NewGuid().ToString();
            string seedUserId = Guid.NewGuid().ToString();
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);            
            Photo seedPhoto = new Photo()
            {
                Id = seedPhotoId,
                Name = seed,
                Author = new User()
                {
                    Id = seedUserId,
                    UserName = expectedUserName
                }
            };
            context.Photos.Add(seedPhoto);
            context.SaveChanges();
            return seedPhoto;
        }        

        [Fact]
        public async Task GetPhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "PhotoRepository_GetPhoto_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                Photo seedPhoto = SeedPhoto(context);
                PhotoRepository photoRepository = new PhotoRepository(context);
                // Act
                Photo result = await photoRepository.GetAsync(seedPhoto.Id);
                // Assert
                Assert.Equal(seedPhoto, result);
            }            
        }
    }
}