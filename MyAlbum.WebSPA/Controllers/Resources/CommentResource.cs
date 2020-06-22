using System;

namespace MyAlbum.WebSPA.Controllers.Resources
{
    public class CommentResource
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
    }
}