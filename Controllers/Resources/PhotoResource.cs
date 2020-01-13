using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyAlbum.Controllers.Resources
{
    public class PhotoResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string FilePath { get; set; }

        public ICollection<CommentResource> Comments { get; set; }
        public ICollection<CategoryResource> PhotoCategories { get; set; }

        public AlbumResource Album { get; set; }

        public UserResource Author { get; set; }

        public PhotoResource()
        {
            Comments = new Collection<CommentResource>();
            PhotoCategories = new Collection<CategoryResource>();
        }        
    }
}