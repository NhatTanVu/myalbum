using System.Threading.Tasks;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}