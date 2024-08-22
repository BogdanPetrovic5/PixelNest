using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string PostDescription { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public ICollection<ImagePath> ImagePaths { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public User User { get; set; }
    }
}
