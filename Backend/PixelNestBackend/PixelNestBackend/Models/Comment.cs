using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PixelNestBackend.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public int TotalLikes { get; set; }
        public string Username { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public ICollection<LikedComments> LikedComments { get; set; }
    }
}
