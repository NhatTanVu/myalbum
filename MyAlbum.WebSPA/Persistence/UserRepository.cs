using System.Threading.Tasks;
using MyAlbum.Core;
using MyAlbum.Core.Models;

namespace MyAlbum.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAlbumDbContext context;
        public UserRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetAsync(string id)
        {
            return await this.context.Users.FindAsync(id);
        }

        public async Task<User> GetOrAdd(User user)
        {
            string id = user.Id;
            User existingUser = await GetAsync(id);
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