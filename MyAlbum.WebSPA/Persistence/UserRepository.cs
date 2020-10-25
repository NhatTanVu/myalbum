using System.Threading.Tasks;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using System.Linq;

namespace MyAlbum.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAlbumDbContext context;
        public UserRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        // public async Task<User> GetAsync(string id)
        // {
        //     return await this.context.Users.FindAsync(id);
        // }

        public User GetByUserName(string userName)
        {
            return this.context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public User GetOrAdd(User user)
        {
            string userName = user.UserName;
            User existingUser = GetByUserName(userName);
            if (existingUser != null)
                return existingUser;
            else
            {
                user.Id = System.Guid.NewGuid().ToString("B");
                this.context.Users.Add(user);
                return user;
            }
        }
    }
}