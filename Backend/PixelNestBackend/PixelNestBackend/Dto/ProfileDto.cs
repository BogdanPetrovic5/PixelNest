namespace PixelNestBackend.Dto
{
    public class ProfileDto
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
