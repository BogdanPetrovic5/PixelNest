﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class LikedPosts
    {

        public int UserID { get; set; }
        public int PostID { get; set; }
        public string Username { get; set; }
        public DateTime DateLiked { get; set; }

        public User? User { get; set; }
        public Post? Post { get; set; }
    }
}