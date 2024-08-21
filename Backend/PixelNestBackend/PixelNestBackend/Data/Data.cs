using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Models;

namespace PixelNestBackend.Data
{
    public class Data : DbContext
    {
        public Data(DbContextOptions<Data> options) :base(options) { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ImagePath> ImagePaths { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
