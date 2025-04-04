﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelNestBackend.Models
{
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoryID { get; set; }
        public int UserID { get; set; }
        
        public Guid StoryGuid { get; set; } = Guid.NewGuid();
        public Guid UserGuid { get; set; }
        [NotMapped]
        public ICollection<ImagePath> ImagePath { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        [NotMapped]
        public User User { get; set; }
        public ICollection<Seen>? SeenList { get; set; }
    }
}
