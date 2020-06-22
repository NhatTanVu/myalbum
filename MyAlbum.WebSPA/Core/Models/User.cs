using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Core.Models
{
    public class User
    {
        public static readonly User AnonymousUser = new User()
        {
            Id = "{559d2e16-f464-4438-85d1-c8ef7776ab27}",
            UserName = "Anonymous",
            FirstName = "Anonymous",
            LastName = string.Empty
        };

        public string Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public ICollection<Album> Albums { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public User()
        {
            Comments = new Collection<Comment>();
            Photos = new Collection<Photo>();
            Albums = new Collection<Album>();
        }
    }
}