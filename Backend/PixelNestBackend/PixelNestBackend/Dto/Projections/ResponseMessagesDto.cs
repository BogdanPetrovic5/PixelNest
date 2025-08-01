﻿namespace PixelNestBackend.Dto.Projections
{
    public class ResponseMessagesDto
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message   { get; set; }
        public int MessageID { get; set; }
        public bool IsSeen { get; set; }
        public string Source { get; set; }
        public string UserID { get; set; }
        public DateTime DateSent { get; set; }
        public bool CanUnsend { get; set; }
    }
}
