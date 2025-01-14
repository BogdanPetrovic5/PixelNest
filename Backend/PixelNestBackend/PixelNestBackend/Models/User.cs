using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PixelNestBackend.Dto;

namespace PixelNestBackend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int TotalLikes { get; set; }
        public int Followers { get; set; } = 0;
        public int Following { get; set; } = 0;
        public int TotalPosts { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Story>? Stories { get; set; }
        [NotMapped]
        public UserProfileDto? UserProfileDto { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikedPosts>? LikedPosts { get; set;}
        public ICollection<LikedComments>? LikedComments { get; set; }
        public ICollection<SavedPosts>? SavedPosts { get; set; }

        public ICollection<Follow>? FollowingsList { get; set; }
        public ICollection<Follow>? FollowersList { get; set; }
        public ICollection<Seen>? SeenList { get; set; }
        public ImagePath? ProfilePhoto { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}
