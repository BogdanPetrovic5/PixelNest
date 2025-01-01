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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Post>().HasKey(p => p.PostID);
            modelBuilder.Entity<ImagePath>().HasKey(p => p.PathID);
            modelBuilder.Entity<Comment>().HasKey(c => c.CommentID);
            modelBuilder.Entity<LikedPosts>().HasKey(lp => new { lp.PostID, lp.UserID });
            modelBuilder.Entity<LikedComments>().HasKey(lc => new { lc.CommentID, lc.UserID });
            modelBuilder.Entity<SavedPosts>().HasKey(lc => new { lc.PostID, lc.UserID });
            modelBuilder.Entity<Follow>().HasKey(f => new {f.UserFollowerID, f.UserFollowingID});
            modelBuilder.Entity<Story>().HasKey(s => s.StoryID);
            modelBuilder.Entity<Seen>().HasKey(s => new {s.StoryID, s.UserID});


            modelBuilder.Entity<Post>()
                .HasOne(user => user.User)
                .WithMany(post => post.Posts)
                .HasForeignKey(user => user.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(post => post.Post)
                .WithMany(comment => comment.Comments)
                .HasForeignKey(post => post.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserID)
               .OnDelete(DeleteBehavior.Cascade);
                

            modelBuilder.Entity<ImagePath>()
                .HasOne(imagePath => imagePath.Post)
                .WithMany(post => post.ImagePaths)
                .HasForeignKey(imagePath => imagePath.PostID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImagePath>()
                .HasOne(imagePath => imagePath.Story)
                .WithMany(story => story.ImagePath)
                .HasForeignKey(ImagePath => ImagePath.StoryID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ImagePath>()
                .Property(pi => pi.PostID)
                .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
              .Property(uid => uid.UserID)
              .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
              .Property(sid => sid.StoryID)
              .IsRequired(false);
            modelBuilder.Entity<ImagePath>()
                .HasOne(user => user.User)
                .WithOne(profilePhoto => profilePhoto.ProfilePhoto)
                .HasForeignKey<ImagePath>(user => user.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikedPosts>()
                .HasOne(user => user.User)
                .WithMany(likedPosts => likedPosts.LikedPosts)
                .HasForeignKey(lp => lp.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikedPosts>()
                .HasOne(post => post.Post)
                .WithMany(likedPosts => likedPosts.LikedPosts)
                .HasForeignKey(lp => lp.PostID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<LikedComments>()
                .HasOne(user => user.User)
                .WithMany(likedComments => likedComments.LikedComments)
                .HasForeignKey(lc => lc.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikedComments>()
                .HasOne(comment => comment.Comment)
                .WithMany(likedComments => likedComments.LikedComments)
                .HasForeignKey(lc => lc.CommentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(post => post.Post)
                .WithMany(savedPosts => savedPosts.SavedPosts)
                .HasForeignKey(sp => sp.PostID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedPosts>()
                .HasOne(user => user.User)
                .WithMany(savedPosts => savedPosts.SavedPosts)
                .HasForeignKey(sp => sp.UserID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Follow>()
                .HasOne(follower => follower.UserFollower)
                .WithMany(followings => followings.FollowingsList)
                .HasForeignKey(follower => follower.UserFollowerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(followed => followed.UserFollowing)
                .WithMany(followings => followings.FollowersList)
                .HasForeignKey(followed => followed.UserFollowingID)               
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Story>()
                .HasOne(user => user.User)
                .WithMany(story => story.Stories)
                .HasForeignKey(user => user.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Seen>()
                .HasOne(user => user.User)
                .WithMany(seen =>  seen.SeenList)
                .HasForeignKey(user => user.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seen>()
                .HasOne(story => story.Story)
                .WithMany(seen => seen.SeenList)
                .HasForeignKey(story => story.StoryID)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
