namespace MyAlbum.Services.Photo.API.Core.Models
{
    public class PhotoQuery
    {
        public int? CategoryId { get; set; }
        public bool? HasLocation { get; set; }
        public string AuthorUserName { get; set; } 
        public int? AlbumId { get; set; }
    }
}