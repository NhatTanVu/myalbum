using System.Threading.Tasks;

namespace MyAlbum.Services.Comment.API.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}