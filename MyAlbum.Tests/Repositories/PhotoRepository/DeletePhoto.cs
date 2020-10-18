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
    public class PhotoRepository_Test5
    {
        private IEnumerable<Photo> SeedPhotos(MyAlbumDbContext context)
        {
            string seedUserId = Guid.NewGuid().ToString();
            string expectedUserName = "test@gmail.com";
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = expectedUserName
            };
            int numOfPhotos = new Random().Next(5, 20);
            List<Photo> seedPhotos = new List<Photo>(numOfPhotos);
            int seedPhotoId = new Random().Next(1, 100);

            for (int i = 1; i <= numOfPhotos; i++)
            {
                Photo seedPhoto = new Photo()
                {
                    Id = seedPhotoId + i,
                    Name = Guid.NewGuid().ToString(),
                    Author = seedUser
                };
                context.Photos.Add(seedPhoto);
                seedPhotos.Add(seedPhoto);
            }
            context.SaveChanges();

            return seedPhotos;
        }  

        [Fact]
        public async Task DeletePhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "DeletePhoto_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                IEnumerable<Photo> seedPhotos = SeedPhotos(context).ToList();
                UnitOfWork unitOfWork = new UnitOfWork(context);
                PhotoRepository photoRepository = new PhotoRepository(context);
                int count = seedPhotos.Count();
                foreach(var photo in seedPhotos)
                {
                    // Act
                    photoRepository.Delete(photo);
                    await unitOfWork.CompleteAsync();
                    // Assert
                    Assert.Equal(--count, context.Photos.Count());
                }
            }            
        }
    }
}