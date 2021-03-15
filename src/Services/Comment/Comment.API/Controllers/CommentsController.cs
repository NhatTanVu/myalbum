using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyAlbum.Services.Comment.API.Core;
using Models = MyAlbum.Services.Comment.API.Core.Models;
using MyAlbum.Services.Comment.API.Controllers.Resources;
using MyAlbum.Services.Comment.API.Hubs;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MyAlbum.Services.Comment.API.Controllers
{
    [ApiExplorerSettings(GroupName = "Comments")]
    [Route("/api/comments")]
    public class CommentsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IHubContext<CommentHub> commentHub;
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        private readonly IPhotoRepository photoRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommentsController(IMapper mapper, IHubContext<CommentHub> commentHub,
            ICommentRepository commentRepository,
            IUserRepository userRepository, IPhotoRepository photoRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.photoRepository = photoRepository;
            this.mapper = mapper;
            this.commentHub = commentHub;
        }

        /// <summary>
        /// Get all replies for a comment by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<CommentResource>), (int)System.Net.HttpStatusCode.OK)]
        public IEnumerable<CommentResource> GetReplies([FromRoute] int id)
        {
            IEnumerable<Models.Comment> replies = this.commentRepository.GetReplies(id);
            var replyResources = mapper.Map<IEnumerable<Models.Comment>, IEnumerable<CommentResource>>(replies);
            return replyResources;
        }

        /// <summary>
        /// Create a comment
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommentResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> CreateComment([FromBody] AddCommentResource commentResource)
        {
            Models.Photo photo = await this.photoRepository.GetAsync(commentResource.PhotoId);
            Models.Comment parentComment = commentResource.ParentId.HasValue ? await commentRepository.GetAsync(commentResource.ParentId.Value) : null;
            bool isParentCommentFromSamePhoto = (parentComment != null) && (parentComment.Photo.Id == commentResource.PhotoId);
            if (photo != null && (!commentResource.ParentId.HasValue || isParentCommentFromSamePhoto))
            {
                var comment = this.mapper.Map<AddCommentResource, Models.Comment>(commentResource);
                var currentUser = new Models.User()
                {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                comment.Author = this.userRepository.GetOrAdd(currentUser);
                comment.Photo = photo;
                this.commentRepository.Add(comment);
                await this.unitOfWork.CompleteAsync();
                CommentResource resourceResult = mapper.Map<Models.Comment, CommentResource>(comment);
                resourceResult.ConnectionId = commentResource.ConnectionId;
                NotifyCommentAdded(comment.Id, commentResource.ConnectionId);

                return Ok(resourceResult);
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Update a comment by ID
        /// </summary>
        [HttpPost("{id}")]
        [Authorize()]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CommentResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromForm] CommentResource commentResource)
        {
            Models.Comment comment = await commentRepository.GetAsync(id);
            if (comment != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (comment.Author.UserName != userName)
                    return Forbid();
                if (string.IsNullOrEmpty(commentResource.Content))
                    return BadRequest();

                comment.Content = commentResource.Content;
                comment.ModifiedDate = DateTime.UtcNow;
                await this.unitOfWork.CompleteAsync();

                var outputCommentResource = mapper.Map<Models.Comment, CommentResource>(comment);
                return Ok(outputCommentResource);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Delete a comment by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize()]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            Models.Comment comment = await commentRepository.GetAsync(id);
            if (comment != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (comment.Author.UserName != userName)
                    return Forbid();

                List<Models.Comment> replies = this.commentRepository.GetReplies(id).ToList();
                this.commentRepository.DeleteAll(replies);
                this.commentRepository.Delete(comment);
                await this.unitOfWork.CompleteAsync();
                return Ok();
            }
            else
                return NotFound();
        }

        private async void NotifyCommentAdded(int id, string connectionId)
        {
            IEnumerable<Models.Comment> replies = this.commentRepository.GetSelfAndAncestors(id);
            List<CommentResource> replyResources = mapper.Map<IEnumerable<Models.Comment>, IEnumerable<CommentResource>>(replies).ToList();
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