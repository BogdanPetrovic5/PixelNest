namespace PixelNestBackend.Models
{
    public class LikedComments
    {
        public int UserID { get; set; }
        public int CommentID { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }

    }
}
