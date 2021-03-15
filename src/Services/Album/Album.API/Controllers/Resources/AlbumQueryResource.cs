namespace MyAlbum.Services.Album.API.Controllers.Resources
{
    public class AlbumQueryResource
    {
        public int? CategoryId { get; set; }
        public bool? HasLocation { get; set; }
        public string AuthorUserName { get; set; }
    }
}