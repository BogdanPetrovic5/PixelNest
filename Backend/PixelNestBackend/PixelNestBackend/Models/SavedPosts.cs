namespace PixelNestBackend.Models
{
    public class SavedPosts
    {
        public int UserID { get; set; }
        public int PostID { get; set; }
        public string Username { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
