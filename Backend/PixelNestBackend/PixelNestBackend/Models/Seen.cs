namespace PixelNestBackend.Models
{
    public class Seen
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int StoryID { get; set; }
        public Story Story { get; set; }
        public User User { get; set; }
    }
}
