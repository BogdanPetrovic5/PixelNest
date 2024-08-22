using Microsoft.EntityFrameworkCore;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Post>().HasKey(p => p.PostID);
            modelBuilder.Entity<ImagePath>().HasKey(p => p.PathID);
            modelBuilder.Entity<Comment>().HasKey(c => c.CommentID);

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

        }
    }
}
