using AutoMapper;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.Core.Models;
using System.Linq;

namespace MyAlbum.WebSPA.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Photo, PhotoResource>();
            CreateMap<Comment, CommentResource>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo.Id))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies.Select(comment => new Comment()
                {
                    Author = comment.Author,
                    Content = comment.Content,
                    Id = comment.Id,
                    NumOfReplies = comment.NumOfReplies,
                    ParentId = comment.ParentId,
                    Photo = comment.Photo
                })));
            CreateMap<PhotoCategory, CategoryResource>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<Album, AlbumResource>();
            CreateMap<User, UserResource>();

            CreateMap<PhotoResource, Photo>();
            CreateMap<CommentResource, Comment>()
                .ConstructUsing((src, ctx) =>
                {
                    return new Comment()
                    {
                        Id = src.Id,
                        Photo = new Photo()
                        {
                            Id = src.PhotoId
                        },
                        Author = ctx.Mapper.Map<UserResource, User>(src.Author)
                    };
                });
            CreateMap<CategoryResource, PhotoCategory>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category()
                {
                    Name = src.Name,
                    Id = src.Id
                }));
            CreateMap<UserResource, User>();
            CreateMap<PhotoQueryResource, PhotoQuery>();
        }
    }
}