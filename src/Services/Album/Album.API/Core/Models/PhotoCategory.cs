using System.ComponentModel.DataAnnotations.Schema;

namespace MyAlbum.Services.Album.API.Core.Models
{
    [Table("PhotoCategories")]
    public class PhotoCategory
    {
        public int PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}