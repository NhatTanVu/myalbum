using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class AlbumRepository_Test2
    {
        private Album SeedAlbum(MyAlbumDbContext context)
        {
            int seedAlbumId = new Random().Next(1, 100);
            string seed = Guid.NewGuid().ToString();
            string seedUserId = Guid.NewGuid().ToString();
            string expectedUserName = string.Format("test_{0}@gmail.com", seed);            
            Album seedAlbum = new Album()
            {
                Id = seedAlbumId,
                Name = seed,
                Author = new User()
                {
                    Id = seedUserId,
                    UserName = expectedUserName
                }
            };
            context.Albums.Add(seedAlbum);
            context.SaveChanges();
            return seedAlbum;
        }        

        [Fact]
        public async Task GetAlbum()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AlbumRepository_GetAlbum_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                Album seedAlbum = SeedAlbum(context);
                AlbumRepository albumRepository = new AlbumRepository(context);
                // Act
                Album result = await albumRepository.GetAsync(seedAlbum.Id);
                // Assert
                Assert.Equal(seedAlbum, result);
            }            
        }        
    }
}