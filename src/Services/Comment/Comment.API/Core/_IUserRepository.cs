namespace MyAlbum.Services.Comment.API.Core
{
    public interface IUserRepository
    {
        Models.User GetOrAdd(Models.User user);
    }
}