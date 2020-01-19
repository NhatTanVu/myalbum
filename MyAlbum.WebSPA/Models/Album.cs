using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public User Author { get; set; }

        public Album()
        {
            Photos = new Collection<Photo>();
        }
    }
}