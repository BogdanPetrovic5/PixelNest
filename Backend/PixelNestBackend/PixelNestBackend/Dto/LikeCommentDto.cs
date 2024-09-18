using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Dto
{
    public class LikeCommentDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int CommentID { get; set; }
    }
}
