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

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(p => p.ExternalProvider)
                .IsRequired()
                .HasMaxLength(50)
                .IsRequired(false);

                entity.Property(p => p.ExternalId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasIndex(p => new
                {
                    p.ExternalProvider,
                    p.ExternalId
                })
                .IsUnique()
                .HasFilter("[ExternalProvider] IS NOT NULL AND [ExternalId] IS NOT NULL");
            });


            modelBuilder.Entity<User>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}