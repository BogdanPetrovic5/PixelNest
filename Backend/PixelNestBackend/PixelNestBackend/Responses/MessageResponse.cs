using Bogus.DataSets;

namespace PixelNestBackend.Responses
{
    public class MessageResponse
    {
        public bool IsSuccessfull { get; set; }
        public bool IsUserInRoom { get; set; }
        public Guid ReceiverID { get; set; }
        public Guid SenderID { get; set; }

        public string Message { get; set; }
        public int MessageID { get; set; }
        public string ReceiverUsername { get; set; }
        public string SenderUsername { get; set; }
        public string ChatID { get; set; }
        public DateTime Date { get; set; }
    }
}
