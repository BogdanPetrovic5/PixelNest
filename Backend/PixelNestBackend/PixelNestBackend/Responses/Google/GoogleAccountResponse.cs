namespace PixelNestBackend.Responses.Google
{
    public class GoogleAccountResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public Guid UserGuid { get; set; }
        public Guid ClientGuid { get; set; }
    }
}
