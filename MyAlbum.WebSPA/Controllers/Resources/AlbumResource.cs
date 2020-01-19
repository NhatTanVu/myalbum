using System.Collections.Generic;

namespace MyAlbum.Controllers.Resources
{
    public class AlbumResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserResource Author { get; set; }

        public AlbumResource()
        {
        }        
    }
}