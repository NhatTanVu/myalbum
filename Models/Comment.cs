namespace MyAlbum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}