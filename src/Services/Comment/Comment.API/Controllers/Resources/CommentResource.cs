using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Comment.API.Controllers.Resources
{
    public class CommentResource : IEquatable<CommentResource>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int PhotoId { get; set; }
        public string Content { get; set; }
        public UserResource Author { get; set; }
        public string ConnectionId { get; set; }
        public int NumOfReplies { get; set; }
        public CommentResource[] Replies { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool Equals([AllowNull] CommentResource other)
        {
            if (other == null)
                return false;

            return (
                this.Id == other.Id &&
                this.Content == other.Content &&
                this.ParentId == other.ParentId &&
                this.PhotoId == other.PhotoId &&
                ((this.Author == null && other.Author == null) || this.Author.Equals(other.Author))
            );
        }

        public override int GetHashCode() => Id;
    }
}