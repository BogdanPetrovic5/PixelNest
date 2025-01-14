namespace PixelNestBackend.Dto.Projections
{
    public class ResponseChatsDto
    {
        public string ChatID { get; set; }
        
        public ICollection<ResponseMessagesDto>Messages { get; set; }
    }
}
