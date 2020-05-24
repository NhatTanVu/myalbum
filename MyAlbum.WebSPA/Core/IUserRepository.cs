using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IUserRepository
    {
         Task<User> GetAsync(string id);
         User GetByUserNameAsync(string userName);
         User GetOrAdd(User user);
    }
}