using AutoMapper;
using MyAlbum.Controllers.Resources;
using MyAlbum.Models;

namespace MyAlbum.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Photo, PhotoResource>();
            CreateMap<Comment, CommentResource>();
            CreateMap<PhotoCategory, CategoryResource>(); // TODO: Need manual map
            CreateMap<Album, AlbumResource>();
            CreateMap<User, UserResource>();
        }
    }
}