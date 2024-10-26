namespace PixelNestBackend.Dto.Projections
{
    public class ResponseImageDto
    {
        public int PathID { get; set; }
        public int? PostID { get; set; }
        public int? StoryID { get; set; }
        public string Path { get; set; }
        public string PhotoDisplay { get; set; }
    }
}
