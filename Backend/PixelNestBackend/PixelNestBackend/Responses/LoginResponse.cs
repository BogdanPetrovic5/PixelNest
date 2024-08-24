namespace PixelNestBackend.Responses
{
    public class LoginResponse
    {
        public string Response { get; set; }
        public string? Token { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
