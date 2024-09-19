using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Dto
{
    public class CommentDto
    {
        [Required]
        public string CommentText { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int PostID { get; set; }
       
        public int? ParentCommentID { get; set; }
      
    }
}
