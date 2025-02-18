namespace PixelNestBackend.Models
{
    public class SavedPosts
    {
        public int UserID { get; set; }
        public int PostID { get; set; }
        public Guid UserGuid { get; set; }
        public Guid PostGuid { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
