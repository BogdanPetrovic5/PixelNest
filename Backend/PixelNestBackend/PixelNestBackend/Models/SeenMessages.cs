namespace PixelNestBackend.Models
{
    public class SeenMessages
    {
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public int SenderID { get; set; }
        public Guid UserGuid { get; set; }
        public Guid SenderGuid { get; set; }
        public User User { get; set; }
        public Message Message { get; set; }
    }
}
