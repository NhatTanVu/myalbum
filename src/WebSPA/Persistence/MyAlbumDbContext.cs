using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;

namespace MyAlbum.Persistence
{
    public class MyAlbumDbContext : DbContext
    {
        public MyAlbumDbContext(DbContextOptions<MyAlbumDbContext> options) : base(options)
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

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentId);

            ConfigureCreatedDateColumn(modelBuilder);
        }

        private void ConfigureCreatedDateColumn(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Comment>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Photo>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<User>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}