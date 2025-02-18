using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public Guid PostGuid { get; set; }
        public Guid UserGuid { get; set; } 
        public string CommentText { get; set; }
        public int? ParentCommentID { get; set; }
        public int TotalLikes { get; set; }
        public int TotalReplies { get; set; } = 0;
        
        public User User { get; set; }
        public Post Post { get; set; }
        [NotMapped]
        public ICollection<ResponseCommentDto>? Replies { get; set; }
        [NotMapped]
        
        public ICollection<LikedComments> LikedComments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
