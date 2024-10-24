namespace PixelNestBackend.Responses
{
    public class StoryResponse
    {
        public bool IsSuccessful { get; set;}
        public string? Message { get; set; }
        public int StoryID { get; set; }
    }
}
