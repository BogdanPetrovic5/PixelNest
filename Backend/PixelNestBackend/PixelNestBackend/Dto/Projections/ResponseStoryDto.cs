namespace PixelNestBackend.Dto.Projections
{
    public class ResponseStoryDto
    {
        public int StoryID { get; set; }
        public string OwnerUsername { get; set; }
        public ICollection<ResponseImageDto>? ImagePaths { get; set; }
    }
}
