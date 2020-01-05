using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string FilePath { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<PhotoCategory> PhotoCategories { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public Photo()
        {
            Comments = new Collection<Comment>();
            PhotoCategories = new Collection<PhotoCategory>();
        }
    }
}