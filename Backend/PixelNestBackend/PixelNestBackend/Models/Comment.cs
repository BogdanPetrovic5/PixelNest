namespace PixelNestBackend.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public int TotalLikes { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }

    }
}
