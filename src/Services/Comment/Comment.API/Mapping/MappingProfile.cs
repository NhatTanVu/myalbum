using AutoMapper;
using MyAlbum.Services.Comment.API.Controllers.Resources;
using Models = MyAlbum.Services.Comment.API.Core.Models;
using System.Linq;

namespace MyAlbum.Services.Comment.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
            CreateMap<Models.User, UserResource>();

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
            CreateMap<AddCommentResource, Models.Comment>()
                .ConstructUsing((src, ctx) =>
                {
                    return new Models.Comment()
                    {
                        Photo = new Models.Photo()
                        {
                            Id = src.PhotoId
                        }
                    };
                });
            CreateMap<UserResource, Models.User>();
        }
    }
}