namespace PixelNestBackend.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string PostDescription { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public ICollection<ImagePath> ImagePaths { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
