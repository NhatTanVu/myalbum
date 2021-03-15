namespace MyAlbum.Services.Photo.API.Core
{
    public interface IUserRepository
    {
        Models.User GetOrAdd(Models.User user);
    }
}