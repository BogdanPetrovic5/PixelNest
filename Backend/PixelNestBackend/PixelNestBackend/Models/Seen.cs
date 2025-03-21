﻿namespace PixelNestBackend.Models
{
    public class Seen
    {
        public int UserID { get; set; }
    
        public int StoryID { get; set; }
        public Guid UserGuid { get; set; } 
        public Guid StoryGuid { get; set; }

        public Story Story { get; set; }
        public User User { get; set; }
    }
}
