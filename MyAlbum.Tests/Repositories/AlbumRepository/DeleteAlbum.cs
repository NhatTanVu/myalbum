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
    public class AlbumRepository_Test3
    {
        private IEnumerable<Album> SeedAlbums(MyAlbumDbContext context)
        {
            string seedUserId = Guid.NewGuid().ToString();
            string expectedUserName = "test@gmail.com";
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = expectedUserName
            };
            int numOfAlbums = new Random().Next(5, 20);
            List<Album> seedAlbums = new List<Album>(numOfAlbums);
            int seedAlbumId = new Random().Next(1, 100);

            for (int i = 1; i <= numOfAlbums; i++)
            {
                Album seedAlbum = new Album()
                {
                    Id = seedAlbumId + i,
                    Name = Guid.NewGuid().ToString(),
                    Author = seedUser
                };
                context.Albums.Add(seedAlbum);
                seedAlbums.Add(seedAlbum);
            }
            context.SaveChanges();

            return seedAlbums;
        }  

        [Fact]
        public async Task DeletePhoto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AlbumRepository_DeleteAlbum_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                IEnumerable<Album> seedAlbums = SeedAlbums(context);
                UnitOfWork unitOfWork = new UnitOfWork(context);
                AlbumRepository albumRepository = new AlbumRepository(context);
                int count = seedAlbums.Count();
                foreach(var album in seedAlbums)
                {
                    // Act
                    albumRepository.Delete(album);
                    await unitOfWork.CompleteAsync();
                    // Assert
                    Assert.Equal(--count, context.Albums.Count());
                }
            }            
        }        
    }
}