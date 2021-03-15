namespace MyAlbum.Services.Album.API.Core
{
    public interface IUserRepository
    {
        Models.User GetOrAdd(Models.User user);
    }
}