using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyAlbum.Core;
using MyAlbum.Core.Models;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.WebSPA.Hubs;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MyAlbum.WebSPA.Controllers
{
    [ApiExplorerSettings(GroupName = "Comments")] 
    [Route("/api/comments")]
    public class CommentsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IHubContext<CommentHub> commentHub;
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoRepository photoRepository;

        public CommentsController(IMapper mapper, IHubContext<CommentHub> commentHub,
            ICommentRepository commentRepository,
            IUserRepository userRepository, IPhotoRepository photoRepository, IUnitOfWork unitOfWork)
        {
            this.photoRepository = photoRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.commentRepository = commentRepository;
            this.mapper = mapper;
            this.commentHub = commentHub;
        }

        /// <summary>
        /// Get all replies for a comment by ID
        /// </summary>
        [HttpGet("{id}")]
        public IEnumerable<CommentResource> GetReplies([FromRoute] int id)
        {
            IEnumerable<Comment> replies = this.commentRepository.GetReplies(id);
            var replyResources = mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResource>>(replies);
            return replyResources;
        }        

        /// <summary>
        /// Create a new comment
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CommentResource commentResource)
        {
            Photo photo = await this.photoRepository.GetAsync(commentResource.PhotoId);
            if (photo != null)
            {
                var comment = this.mapper.Map<CommentResource, Comment>(commentResource);
                var currentUser = new User() {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                comment.Author = this.userRepository.GetOrAdd(currentUser);
                comment.Photo = photo;
                this.commentRepository.Add(comment);
                await this.unitOfWork.CompleteAsync();
                CommentResource resourceResult = mapper.Map<Comment, CommentResource>(comment);
                resourceResult.ConnectionId = commentResource.ConnectionId;
                NotifyCommentAdded(comment.Id, commentResource.ConnectionId);

                return Ok(resourceResult);
            }
            else
                return NoContent();
        }

        private async void NotifyCommentAdded(int id, string connectionId)
        {
            IEnumerable<Comment> replies = this.commentRepository.GetSelfAndAncestors(id);
            List<CommentResource> replyResources = mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResource>>(replies).ToList();
            foreach (var resource in replyResources)
            {
                resource.ConnectionId = connectionId;
            }
            for (int i = 0; i < replyResources.Count - 1; i++)
            {
                var child = replyResources[i];
                var parent = replyResources[i + 1];
                parent.Replies[Array.FindIndex(parent.Replies, r => r.Id == child.Id)] = child;
            }
            var comment = replyResources[replyResources.Count - 1];
            await this.commentHub.Clients.All.SendAsync("commentAdded", comment);
        }
    }
}