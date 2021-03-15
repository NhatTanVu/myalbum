using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAlbum.Services.Comment.API.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
        public Photo Photo { get; set; }
        public User Author { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }

        [NotMapped]
        public int NumOfReplies { get; set; }
        public virtual Comment Parent { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}