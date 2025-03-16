namespace PixelNestBackend.Dto
{
    public class LikeDto
    {
        public string ClientGuid { get; set; }
        public string Username { get; set; }
        public Guid PostID { get; set; }
    }
}
