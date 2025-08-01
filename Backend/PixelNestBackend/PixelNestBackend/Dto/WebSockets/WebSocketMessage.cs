﻿namespace PixelNestBackend.Dto.WebSockets
{
    public class WebSocketMessage
    {
        public string Type { get; set; }
        public string RoomID { get; set; }
        public string ChatID { get; set; }
        public string TargetUser { get; set; }
        public int MessageID { get; set; }
        public string Content { get; set; }
        public string SenderUsername { get; set; }
        public string SenderUser { get; set; }
        public DateTime Date { get; set; }
    }
}
