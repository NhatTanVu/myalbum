using System.Threading.Tasks;

namespace MyAlbum.Services.Album.API.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}