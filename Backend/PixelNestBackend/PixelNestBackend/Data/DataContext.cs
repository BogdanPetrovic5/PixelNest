using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ImagePath> ImagePaths { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikedPosts> LikedPosts { get; set; }
        public DbSet<LikedComments> LikeComments { get; set; }
        public DbSet<SavedPosts> SavedPosts { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Seen> Seen { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SeenMessages> SeenMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>().HasKey(u => u.UserGuid);
            modelBuilder.Entity<Post>().HasKey(p => p.PostGuid);
            modelBuilder.Entity<ImagePath>().HasKey(p => p.PathID);
            modelBuilder.Entity<Comment>().HasKey(c => c.CommentID);
            modelBuilder.Entity<LikedPosts>().HasKey(lp => new { lp.PostGuid, lp.UserGuid });
           
            modelBuilder.Entity<LikedComments>().HasKey(lc => new { lc.CommentID, lc.UserGuid });
            modelBuilder.Entity<SavedPosts>().HasKey(lc => new { lc.PostGuid, lc.UserGuid });
            modelBuilder.Entity<Follow>().HasKey(f => new {f.UserFollowerGuid, f.UserFollowingGuid});
            modelBuilder.Entity<Story>().HasKey(s => s.StoryGuid);
            modelBuilder.Entity<Seen>().HasKey(s => new {s.StoryGuid, s.UserGuid});
            modelBuilder.Entity<SeenMessages>().HasKey(s => new { s.UserGuid, s.MessageID });
            modelBuilder.Entity<Notification>().HasKey(n => new { n.NotificaitonID });

            modelBuilder.Entity<User>().HasIndex(u => new {u.Email, u.UserGuid, u.Username});
            modelBuilder.Entity<Post>().HasIndex(p => p.UserGuid);
            modelBuilder.Entity<Comment>().HasIndex(c => c.ParentCommentID);
            modelBuilder.Entity<Seen>().HasIndex(s => s.UserGuid);
            modelBuilder.Entity<Story>().HasIndex(s => s.UserGuid);

            modelBuilder.Entity<Notification>().HasIndex(n => new { n.ReceiverGuid, n.SenderGuid, n.PostGuid });

            modelBuilder.Entity<Notification>()
                .HasOne(user => user.SenderUser)
                .WithMany(notifications => notifications.SentNotifications)
                .HasForeignKey(user => user.SenderGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>()
               .HasOne(user => user.ReceiverUser)
               .WithMany(notifications => notifications.ReceivedNotifications)
               .HasForeignKey(user => user.ReceiverGuid)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>()
              .HasOne(post => post.Post)
              .WithMany(notifications => notifications.Notifications)
              .HasForeignKey(post => post.PostGuid)
              .OnDelete(DeleteBehavior.Restrict);
        
           

            modelBuilder.Entity<SeenMessages>()
                .HasOne(user => user.User)
                .WithMany(seen => seen.SeenMessages)
                .HasForeignKey(s => s.UserGuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeenMessages>()
                .HasOne(message => message.Message)
                .WithMany(seen => seen.SeenByUsers)
                .HasForeignKey(s => s.MessageID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(user => user.Sender)
                .WithMany(message => message.SentMessages)
                .HasForeignKey(user => user.SenderGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
               .HasOne(user => user.Receiver)
               .WithMany(message => message.ReceivedMessages)
               .HasForeignKey(user => user.ReceiverGuid)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(user => user.User)
                .WithMany(post => post.Posts)
                .HasForeignKey(user => user.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(post => post.Post)
                .WithMany(comment => comment.Comments)
                .HasForeignKey(post => post.PostGuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserGuid)
               .OnDelete(DeleteBehavior.Cascade);
                

            modelBuilder.Entity<ImagePath>()
                .HasOne(imagePath => imagePath.Post)
                .WithMany(post => post.ImagePaths)
                .HasForeignKey(imagePath => imagePath.PostGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImagePath>()
                .HasOne(imagePath => imagePath.Story)
                .WithMany(story => story.ImagePath)
                .HasForeignKey(ImagePath => ImagePath.StoryGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ImagePath>()
                .Property(pi => pi.PostGuid)
                .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
              .Property(uid => uid.UserGuid)
              .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
              .Property(sid => sid.StoryGuid)
              .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
                .HasOne(user => user.User)
                .WithOne(profilePhoto => profilePhoto.ProfilePhoto)
                .HasForeignKey<ImagePath>(user => user.UserGuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikedPosts>()
                .HasOne(user => user.User)
                .WithMany(likedPosts => likedPosts.LikedPosts)
                .HasForeignKey(lp => lp.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikedPosts>()
                .HasOne(post => post.Post)
                .WithMany(likedPosts => likedPosts.LikedPosts)
                .HasForeignKey(lp => lp.PostGuid)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<LikedComments>()
                .HasOne(user => user.User)
                .WithMany(likedComments => likedComments.LikedComments)
                .HasForeignKey(lc => lc.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikedComments>()
                .HasOne(comment => comment.Comment)
                .WithMany(likedComments => likedComments.LikedComments)
                .HasForeignKey(lc => lc.CommentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(post => post.Post)
                .WithMany(savedPosts => savedPosts.SavedPosts)
                .HasForeignKey(sp => sp.PostGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(user => user.User)
                .WithMany(savedPosts => savedPosts.SavedPosts)
                .HasForeignKey(sp => sp.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Follow>()
                .HasOne(follower => follower.UserFollower)
                .WithMany(followings => followings.FollowingsList)
                .HasForeignKey(follower => follower.UserFollowerGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(followed => followed.UserFollowing)
                .WithMany(followings => followings.FollowersList)
                .HasForeignKey(followed => followed.UserFollowingGuid)               
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Story>()
                .HasOne(user => user.User)
                .WithMany(story => story.Stories)
                .HasForeignKey(user => user.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Seen>()
                .HasOne(user => user.User)
                .WithMany(seen =>  seen.SeenList)
                .HasForeignKey(user => user.UserGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seen>()
                .HasOne(story => story.Story)
                .WithMany(seen => seen.SeenList)
                .HasForeignKey(story => story.StoryGuid)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
