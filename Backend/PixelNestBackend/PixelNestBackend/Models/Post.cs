using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PixelNestBackend.Dto;
using System.Runtime.InteropServices;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int PostID { get; set; }
        public int UserID { get; set; }
        
        public Guid PostGuid { get; set; } = Guid.NewGuid();
        public Guid UserGuid {  get; set; } 
        public string? PostDescription { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }

        public string? Location { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        [NotMapped]
        public ICollection<LikeDto>? LikedByUsers { get; set; }
        [NotMapped]
        public ICollection<ResponseCommentDto> AllComments { get; set; }
        [NotMapped]
        public ICollection<SavePostDto> SavedByUsers { get; set; }

        public ICollection<ImagePath>? ImagePaths { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikedPosts>? LikedPosts { get; set; }
        public ICollection<SavedPosts> SavedPosts { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    
    }
}
