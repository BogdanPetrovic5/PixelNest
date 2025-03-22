namespace PixelNestBackend.Responses.Google
{
    public class GoogleLoginResponse
    {
        public string Response { get; set; }
        public Guid ClientGuid { get; set; }
        public Guid? UserGuid { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
