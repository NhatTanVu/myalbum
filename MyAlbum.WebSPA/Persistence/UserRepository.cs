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
    }
}