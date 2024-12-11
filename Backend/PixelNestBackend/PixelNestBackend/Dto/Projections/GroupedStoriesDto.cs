namespace PixelNestBackend.Dto.Projections
{
    public class GroupedStoriesDto
    {
        public string OwnerUsername { get; set; }
        public int StoriesLeft { get; set; } 
        public List<ResponseStoryDto> Stories { get; set; }
    }
}
