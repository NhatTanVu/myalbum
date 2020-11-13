using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class UserRepository_Test1
    {
        private User SeedUser(MyAlbumDbContext context)
        {
            string seedUserId = Guid.NewGuid().ToString();
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = string.Format("test_{0}@gmail.com", seedUserId),
                FirstName = string.Format("FN_{0}", seedUserId),
                LastName = string.Format("LN_{0}", seedUserId),
            };
            context.Users.Add(seedUser);
            context.SaveChanges();
            return seedUser;
        }        

        [Fact]
        public void GetByUserName()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRepository_GetByUserName_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                var seedUser = SeedUser(context);
                var userRepository = new UserRepository(context);
                // Act
                var result = userRepository.GetByUserName(seedUser.UserName);
                // Assert
                Assert.Equal(seedUser, result);
            }            
        }
    }
}