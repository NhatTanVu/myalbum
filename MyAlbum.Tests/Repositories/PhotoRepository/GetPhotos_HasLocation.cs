using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class PhotoRepository_Test4
    {
        private bool RandBoolean()
        {
            return new Random().NextDouble() > 0.5;
        }

        private IEnumerable<Photo> SeedPhotos_HasLocation(MyAlbumDbContext context)
        {
            string seedUserId = Guid.NewGuid().ToString();
            string expectedUserName = "test@gmail.com";
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = expectedUserName
            };
            int numOfPhotos = new Random().Next(1, 20);
            List<Photo> seedPhotos = new List<Photo>(numOfPhotos);
            int seedPhotoId = new Random().Next(1, 100);

            for (int i = 1; i <= numOfPhotos; i++)
            {
                Photo seedPhoto = new Photo()
                {
                    Id = seedPhotoId + i,
                    Name = Guid.NewGuid().ToString(),
                    Author = seedUser,
                };

                if (RandBoolean()) {
                    seedPhoto.LocLng = seedPhoto.LocLat = seedPhotoId;
                    seedPhotos.Add(seedPhoto);
                }
                context.Photos.Add(seedPhoto);
            }
            context.SaveChanges();

            return seedPhotos;
        }        

        [Fact]
        public async Task GetPhotos_HasLocation()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "PhotoRepository_GetPhotos_HasLocation_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                IEnumerable<Photo> seedPhotos = SeedPhotos_HasLocation(context);
                PhotoRepository photoRepository = new PhotoRepository(context);
                PhotoQuery filter = new PhotoQuery() {
                    HasLocation = true
                };
                // Act
                var photos = await photoRepository.GetPhotos(filter);
                // Assert
                Assert.Equal(seedPhotos, photos);
            }            
        }
    }
}