﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificaitonID { get; set; }
        public int ReceiverID { get; set; }
        public int SenderID { get; set; }
        public int? PostID { get; set; }
        public int? LikeID { get; set; }
        public int? CommentID { get; set; }
        public int? ParentCommentID { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public User ReceiverUser { get; set; }
        public User SenderUser { get; set; }
        public Post Post { get; set; }
     
        
        
    }
}
