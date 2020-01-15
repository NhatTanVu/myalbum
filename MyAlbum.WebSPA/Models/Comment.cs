namespace MyAlbum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public Photo Photo { get; set; }

        public User Author { get; set; }
    }
}