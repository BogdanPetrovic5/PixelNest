namespace PixelNestBackend.Responses
{
    public class FollowResponse
    {
        public bool IsFollowing { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsDuplicate { get; set; }
        public string User { get; set; }
        public string TargetUser { get; set; }
    }
}
