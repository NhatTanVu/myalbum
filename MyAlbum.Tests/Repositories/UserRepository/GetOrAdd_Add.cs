using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class UserRepository_Test3
    {
        private User SeedNewUser()
        {
            string seedUserId = Guid.NewGuid().ToString();
            User seedUser = new User()
            {
                Id = seedUserId,
                UserName = string.Format("test_{0}@gmail.com", seedUserId),
                FirstName = string.Format("FN_{0}", seedUserId),
                LastName = string.Format("LN_{0}", seedUserId),
            };
            return seedUser;
        }

        [Fact]
        public void GetOrAdd_Add()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRepository_GetOrAdd_Add_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                var newUser = SeedNewUser();
                var userRepository = new UserRepository(context);
                // Act
                var result = userRepository.GetOrAdd(newUser);
                // Assert
                Assert.Equal(newUser, result);
            }         
        }
    }
}