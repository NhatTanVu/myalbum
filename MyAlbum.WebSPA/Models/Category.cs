using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAlbum.Models
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PhotoCategory> PhotoCategories { get; set; }

        public Category()
        {
            PhotoCategories = new Collection<PhotoCategory>();
        }
    }
}