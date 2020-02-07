using AutoMapper;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.Core.Models;

namespace MyAlbum.WebSPA.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Photo, PhotoResource>();
            CreateMap<Comment, CommentResource>();
            CreateMap<PhotoCategory, CategoryResource>()
                .ForMember(res => res.Name, opt => opt.MapFrom(cat => cat.Category.Name))
                .ForMember(res => res.Id, opt => opt.MapFrom(cat => cat.Category.Id));
            CreateMap<Album, AlbumResource>();
            CreateMap<User, UserResource>();
            CreateMap<PhotoResource, Photo>();
            CreateMap<PhotoQueryResource, PhotoQuery>();
        }
    }
}