namespace PixelNestBackend.Responses
{
    public class MessageResponse
    {
        public bool IsSuccessfull { get; set; }
        public Guid ReceiverID { get; set; }
        public Guid SenderID { get; set; }

        public string Message { get; set; }
        public string ReceiverUsername { get; set; }
        public string SenderUsername { get; set; }
    }
}
