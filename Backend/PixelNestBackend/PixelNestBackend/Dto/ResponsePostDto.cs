﻿using PixelNestBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelNestBackend.Dto
{
    public class ResponsePostDto
    {
        public int PostID { get; set; }

        public string OwnerUsername { get; set; }

        public string PostDescription { get; set; }

        public int TotalLikes { get; set; }

        public int TotalComments { get; set; }

        public DateTime PublishDate { get; set; }

     

        public ICollection<LikeDto> LikedByUsers { get; set; }
        public ICollection<SavePostDto> SavedByUsers { get; set; }

        public ICollection<ImagePath> ImagePaths { get; set; }
    }
}