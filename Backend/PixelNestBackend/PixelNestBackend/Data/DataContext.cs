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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikeDto>().HasNoKey();
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Post>().HasKey(p => p.PostID);
            modelBuilder.Entity<ImagePath>().HasKey(p => p.PathID);
            modelBuilder.Entity<Comment>().HasKey(c => c.CommentID);
            modelBuilder.Entity<LikedPosts>().HasKey(lp => new { lp.PostID, lp.UserID });
            modelBuilder.Entity<LikedComments>().HasKey(lc => new { lc.CommentID, lc.UserID });

            modelBuilder.Entity<Post>()
                .HasOne(user => user.User)
                .WithMany(post => post.Posts)
                .HasForeignKey(user => user.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(post => post.Post)
                .WithMany(comment => comment.Comments)
                .HasForeignKey(post => post.PostID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImagePath>()
                .HasOne(post => post.Post)
                .WithMany(path => path.ImagePaths)
                .HasForeignKey(post => post.PostID)
                .OnDelete(DeleteBehavior.Restrict);

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
        }
    }
}
