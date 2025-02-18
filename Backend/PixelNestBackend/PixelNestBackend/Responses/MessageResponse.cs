namespace PixelNestBackend.Responses
{
    public class MessageResponse
    {
        public bool IsSuccessfull { get; set; }
        public Guid ReceiverID { get; set; }
        public Guid SenderID { get; set; }
    }
}
