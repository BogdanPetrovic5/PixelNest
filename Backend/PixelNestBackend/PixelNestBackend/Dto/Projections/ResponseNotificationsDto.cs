namespace PixelNestBackend.Dto.Projections
{
    public class ResponseNotificationsDto
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int? PostID { get; set; }
        public int NotificationID { get; set; }
        public ICollection<ResponseImageDto> ImagePath { get; set; }
    }
}
