using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class AlbumRepository_Test1
    {
        [Fact]
        public async Task AddAlbum()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "AlbumRepository_AddAlbum_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                AlbumRepository albumRepository = new AlbumRepository(context);
                string seed = Guid.NewGuid().ToString();
                string seedUserId = Guid.NewGuid().ToString();
                int seedAlbumId = new Random().Next(1, 100);
                string expectedUserName = string.Format("test_{0}@gmail.com", seed);                
                Album originalAlbum = new Album()
                {
                    Id = seedAlbumId,
                    Author = new User()
                    {
                        Id = seedUserId,
                        UserName = expectedUserName
                    }
                };
                // Act
                albumRepository.Add(originalAlbum);
                await unitOfWork.CompleteAsync();
                // Assert
                Assert.Equal(1, await context.Albums.CountAsync(c => true));
                var savedAlbum = await context.Albums.FirstAsync(c => true);
                Assert.NotEqual(0, savedAlbum.Id);
                Assert.Equal(originalAlbum.Name, savedAlbum.Name);
                Assert.Equal(originalAlbum.Author.Id, savedAlbum.Author.Id);
                Assert.Equal(originalAlbum.Author.UserName, savedAlbum.Author.UserName);  
            }          
        }        
    }
}