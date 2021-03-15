using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Comment.API.Controllers.Resources
{
    public class AddCommentResource : IEquatable<AddCommentResource>
    {
        public int? ParentId { get; set; }
        public int PhotoId { get; set; }
        public string Content { get; set; }
        public string ConnectionId { get; set; }

        public bool Equals([AllowNull] AddCommentResource other)
        {
            if (other == null)
                return false;

            return (
                this.Content == other.Content &&
                this.ParentId == other.ParentId &&
                this.PhotoId == other.PhotoId
            );
        }
    }
}