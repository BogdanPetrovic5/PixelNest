namespace PixelNestBackend.Models
{
    public class ImagePath
    {
        public string Path{ get; set; }
        public int PostID { get; set; }
        public Post Post { get; set; }
    }
}
