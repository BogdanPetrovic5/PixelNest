namespace PixelNestBackend.Dto.Projections
{
    public class ResponseStoryDto
    {
        public Guid StoryID { get; set; }
        public string OwnerUsername { get; set; }
        public bool SeenByUser { get; set; }
        public ICollection<ResponseImageDto>? ImagePaths { get; set; }
    }
}
