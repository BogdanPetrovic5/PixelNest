namespace PixelNestBackend.Dto
{
    public class UserProfileDto
    {
        public int Followings { get; set; }
        public int Followers { get; set; }
        public int TotalPosts { get; set; }
        public string Username { get; set; }
    }
}
