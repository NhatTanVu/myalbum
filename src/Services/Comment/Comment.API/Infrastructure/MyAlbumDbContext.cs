using Microsoft.EntityFrameworkCore;
using Models = MyAlbum.Services.Comment.API.Core.Models;

namespace MyAlbum.Services.Comment.API.Infrastructure
{
    public class MyAlbumDbContext : DbContext
    {
        public MyAlbumDbContext(DbContextOptions<MyAlbumDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Comment> Comments { get; set; }
        public DbSet<Models.Photo> Photos { get; set; }
        public DbSet<Models.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.PhotoCategory>()
            .HasKey(p => new { p.PhotoId, p.CategoryId });

            modelBuilder.Entity<Models.PhotoCategory>()
                .HasOne(pc => pc.Photo)
                .WithMany(p => p.PhotoCategories)
                .HasForeignKey(pc => pc.PhotoId);

            modelBuilder.Entity<Models.PhotoCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PhotoCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<Models.Comment>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentId);

            ConfigureCreatedDateColumn(modelBuilder);
        }

        private void ConfigureCreatedDateColumn(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Comment>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Models.Photo>()
                            .Property(b => b.CreatedDate)
                            .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Models.User>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}