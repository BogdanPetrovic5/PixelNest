﻿namespace PixelNestBackend.Dto
{
    public class PostDto
    {
        public string? PostDescription { get; set; }
        public string OwnerUsername { get; set; }
        public string PhotoDisplay { get; set; }
        public string? Location { get; set; }
        public List<IFormFile> Photos { get; set; } 
    }
}
