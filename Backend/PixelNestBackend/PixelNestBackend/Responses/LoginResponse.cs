namespace PixelNestBackend.Responses
{
    public class LoginResponse
    {
        public string Response { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
