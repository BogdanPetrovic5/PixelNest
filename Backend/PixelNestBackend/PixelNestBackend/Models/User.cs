﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PixelNestBackend.Dto;

namespace PixelNestBackend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public Guid UserGuid { get; set; } = Guid.NewGuid();
        public Guid ClientGuid {  get; set; } = Guid.NewGuid();
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public int TotalLikes { get; set; }
        public int Followers { get; set; } = 0;
        public int Following { get; set; } = 0;
        public int TotalPosts { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
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
        public ICollection<SeenMessages> SeenMessages { get; set; }
        public ICollection<Notification> ReceivedNotifications { get; set; }

        public ICollection<Notification> SentNotifications { get; set; }
    }
}
