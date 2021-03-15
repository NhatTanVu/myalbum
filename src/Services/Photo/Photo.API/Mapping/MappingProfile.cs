using AutoMapper;
using MyAlbum.Services.Photo.API.Controllers.Resources;
using Models = MyAlbum.Services.Photo.API.Core.Models;
using System.Linq;

namespace MyAlbum.Services.Photo.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Photo, PhotoResource>();
            CreateMap<Models.Comment, CommentResource>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo.Id))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies.Select(comment => new Models.Comment()
                {
                    Author = comment.Author,
                    Content = comment.Content,
                    Id = comment.Id,
                    NumOfReplies = comment.NumOfReplies,
                    ParentId = comment.ParentId,
                    Photo = comment.Photo
                })));
            CreateMap<Models.PhotoCategory, CategoryResource>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<Models.Album, AlbumResource>();
            CreateMap<Models.User, UserResource>();

            CreateMap<PhotoResource, Models.Photo>();
            CreateMap<CommentResource, Models.Comment>()
                .ConstructUsing((src, ctx) =>
                {
                    return new Models.Comment()
                    {
                        Id = src.Id,
                        Photo = new Models.Photo()
                        {
                            Id = src.PhotoId
                        },
                        Author = ctx.Mapper.Map<UserResource, Models.User>(src.Author)
                    };
                });
            CreateMap<CategoryResource, Models.PhotoCategory>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Models.Category()
                {
                    Name = src.Name,
                    Id = src.Id
                }));
            CreateMap<AlbumResource, Models.Album>();
            CreateMap<UserResource, Models.User>();
            CreateMap<PhotoQueryResource, Models.PhotoQuery>();
        }
    }
}