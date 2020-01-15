using Microsoft.EntityFrameworkCore;
using MyAlbum.Models;

namespace MyAlbum.Persistence
{
    public class MyAlbumDbContext : DbContext
    {
        public MyAlbumDbContext(DbContextOptions<MyAlbumDbContext> options): base(options)
        {
        }
             
        public DbSet<Album> Albums { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoCategory>()
            .HasKey(p => new { p.PhotoId, p.CategoryId });

            modelBuilder.Entity<PhotoCategory>()
                .HasOne(pc => pc.Photo)
                .WithMany(p => p.PhotoCategories)
                .HasForeignKey(pc => pc.PhotoId);

            modelBuilder.Entity<PhotoCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PhotoCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
    }
}