namespace PixelNestBackend.Dto
{
    public class StoryDto
    {
        public string Username { get; set; }
        public string PhotoDisplay { get; set; }
        
        public IFormFile StoryImage { get; set; }
    }
}
