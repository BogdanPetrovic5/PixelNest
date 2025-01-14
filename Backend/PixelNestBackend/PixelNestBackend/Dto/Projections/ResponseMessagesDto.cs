namespace PixelNestBackend.Dto.Projections
{
    public class ResponseMessagesDto
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message   { get; set; }
        public string Source { get; set; }
        public DateTime DateSent { get; set; }
    }
}
