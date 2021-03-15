using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Services.Photo.API.Core.Models
{
    public class User
    {
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