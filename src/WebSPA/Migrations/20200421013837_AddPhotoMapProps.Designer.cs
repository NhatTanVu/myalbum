﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAlbum.Persistence;

namespace MyAlbum.Migrations
{
    [DbContext(typeof(MyAlbumDbContext))]
    [Migration("20200421013837_AddPhotoMapProps")]
    partial class AddPhotoMapProps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyAlbum.Core.Models.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhotoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PhotoId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumId")
                        .HasColumnType("int");

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("CenterLat")
                        .HasColumnType("float");

                    b.Property<double?>("CenterLng")
                        .HasColumnType("float");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<double?>("LocLat")
                        .HasColumnType("float");

                    b.Property<double?>("LocLng")
                        .HasColumnType("float");

                    b.Property<string>("MapFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MapZoom")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.PhotoCategory", b =>
                {
                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("PhotoId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PhotoCategories");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Album", b =>
                {
                    b.HasOne("MyAlbum.Core.Models.User", "Author")
                        .WithMany("Albums")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Comment", b =>
                {
                    b.HasOne("MyAlbum.Core.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId");

                    b.HasOne("MyAlbum.Core.Models.Photo", "Photo")
                        .WithMany("Comments")
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.Photo", b =>
                {
                    b.HasOne("MyAlbum.Core.Models.Album", "Album")
                        .WithMany("Photos")
                        .HasForeignKey("AlbumId");

                    b.HasOne("MyAlbum.Core.Models.User", "Author")
                        .WithMany("Photos")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("MyAlbum.Core.Models.PhotoCategory", b =>
                {
                    b.HasOne("MyAlbum.Core.Models.Category", "Category")
                        .WithMany("PhotoCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyAlbum.Core.Models.Photo", "Photo")
                        .WithMany("PhotoCategories")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}