namespace MyAlbum.Controllers.Resources
{
    public class CommentResource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public UserResource Author { get; set; }
    }
}