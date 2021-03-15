namespace MyAlbum.Services.Photo.API.Controllers.Resources
{
    public class PhotoQueryResource
    {
        public int? CategoryId { get; set; }
        public bool? HasLocation { get; set; }
        public string AuthorUserName { get; set; } 
        public int? AlbumId { get; set; }
    }
}