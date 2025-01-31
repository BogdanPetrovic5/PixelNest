namespace PixelNestBackend.Responses
{
    public class PostResponse
    {
        public int PostID { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
        public string? User { get; set; }
        public string? TargetUser { get; set; }
        public bool DoubleAction { get; set; }
    }
}
