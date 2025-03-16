using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Dto
{
    public class CommentDto
    {
        [Required]
        public string CommentText { get; set; }
        
        [Required]
        public Guid PostID { get; set; }
       
        public int? ParentCommentID { get; set; }
      
    }
}
