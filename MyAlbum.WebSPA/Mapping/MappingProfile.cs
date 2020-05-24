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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<Album, AlbumResource>();
            CreateMap<User, UserResource>();

            CreateMap<PhotoResource, Photo>();
            CreateMap<CommentResource, Comment>()
                .ConstructUsing((src, ctx) =>
                {
                    return new Comment() {
                        Id = src.Id,
                        Photo = new Photo() {
                            Id = src.PhotoId
                        },
                        Author = ctx.Mapper.Map<UserResource, User>(src.Author)
                    };
                });
            CreateMap<UserResource, User>();
            CreateMap<PhotoQueryResource, PhotoQuery>();
        }
    }
}