namespace MyAlbum.Core.Models
{
    public class AlbumQuery
    {
        public int? CategoryId { get; set; }
        public bool? HasLocation { get; set; }
        public string AuthorUserName { get; set; }        
    }
}