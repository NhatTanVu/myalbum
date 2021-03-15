using MyAlbum.Services.Comment.API.Core;
using Models = MyAlbum.Services.Comment.API.Core.Models;
using System.Linq;

namespace MyAlbum.Services.Comment.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAlbumDbContext context;
        public UserRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public Models.User GetOrAdd(Models.User user)
        {
            string userName = user.UserName;
            Models.User existingUser = GetByUserName(userName);
            if (existingUser != null)
                return existingUser;
            else
            {
                user.Id = System.Guid.NewGuid().ToString("B");
                this.context.Users.Add(user);
                return user;
            }
        }

        private Models.User GetByUserName(string userName)
        {
            return this.context.Users.FirstOrDefault(u => u.UserName == userName);
        }
    }
}