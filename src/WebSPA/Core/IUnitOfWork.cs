using System.Threading.Tasks;

namespace MyAlbum.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}