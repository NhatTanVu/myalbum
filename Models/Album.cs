using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public Album()
        {
            Photos = new Collection<Photo>();
        }
    }
}