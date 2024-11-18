using Microsoft.EntityFrameworkCore;
using BlogDALLibrary.Models;
using System.IO;

namespace BlogDALLibrary
{
    public class HedonismBlogContext : DbContext
    {
        public HedonismBlogContext(DbContextOptions<HedonismBlogContext> options)
: base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "BlogDALLibrary", "App_Data", "HedonismBlog.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        public DbSet<User> Users { get; set; }
        public  DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
