namespace PixelNestBackend.Dto
{
    public class UserProfileDto
    {
        public int Followings { get; set; }
        public int Followers { get; set; }
        public int TotalPosts { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public ICollection<FollowDto> ListFollowings { get; set; } 
        public ICollection<FollowDto> ListFollowers { get; set; }
    }
}
