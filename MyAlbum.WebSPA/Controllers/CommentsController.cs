using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using MyAlbum.WebSPA.Controllers.Resources;

namespace MyAlbum.WebSPA.Controllers
{
    [Route("/api/comments")]
    public class CommentsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoRepository photoRepository;

        public CommentsController(IMapper mapper, ICommentRepository commentRepository,

        IUserRepository userRepository, IPhotoRepository photoRepository, IUnitOfWork unitOfWork)
        {
            this.photoRepository = photoRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Add a new comment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentResource commentResource)
        {
            Photo photo = await this.photoRepository.GetAsync(commentResource.PhotoId);
            if (photo != null)
            {
                var comment = this.mapper.Map<CommentResource, Comment>(commentResource);
                comment.Author = this.userRepository.GetOrAdd(comment.Author);
                comment.Photo = photo;
                this.commentRepository.Add(comment);
                await this.unitOfWork.CompleteAsync();

                return Ok(mapper.Map<Comment, CommentResource>(comment));
            }
            else
                return NoContent();
        }
    }
}