using System.Threading.Tasks;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface IUserRepository
    {
         User GetByUserName(string userName);
         User GetOrAdd(User user);
    }
}