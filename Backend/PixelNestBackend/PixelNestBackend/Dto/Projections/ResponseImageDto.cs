namespace PixelNestBackend.Dto.Projections
{
    public class ResponseImageDto
    {
        public int PathID { get; set; }
        public Guid PostID { get; set; }
        public Guid StoryID { get; set; }
        public string Path { get; set; }
        public string PhotoDisplay { get; set; }
    }
}
