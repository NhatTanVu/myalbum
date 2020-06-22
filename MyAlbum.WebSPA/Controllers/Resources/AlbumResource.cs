using System;

namespace MyAlbum.WebSPA.Controllers.Resources
{
    public class AlbumResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserResource Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public AlbumResource()
        {
        }
    }
}