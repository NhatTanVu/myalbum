using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.Services.Album.API.Controllers.Resources
{
    public class PhotoResource : IEquatable<PhotoResource>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string FilePath { get; set; }
        public string BoundingBoxFilePath { get; set; }
        public IFormFile FileToUpload { get; set; }
        public double? LocLng { get; set; }
        public double? LocLat { get; set; }
        public double? CenterLng { get; set; }
        public double? CenterLat { get; set; }
        public int? MapZoom { get; set; }
        public string MapFilePath { get; set; } // NOTE: For reservation only
        public IEnumerable<CommentResource> Comments { get; set; }
        public int TotalComments { get; set; }
        public IEnumerable<CategoryResource> PhotoCategories { get; set; }
        public AlbumResource Album { get; set; }
        public UserResource Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public PhotoResource()
        {
            Comments = new Collection<CommentResource>();
            PhotoCategories = new Collection<CategoryResource>();
        }

        public bool Equals([AllowNull] PhotoResource other)
        {
            if (other == null)
                return false;

            return (
                this.Id == other.Id &&
                this.Name == other.Name &&
                this.Width == other.Width &&
                this.Height == other.Height &&
                this.FilePath == other.FilePath &&
                this.BoundingBoxFilePath == other.BoundingBoxFilePath &&
                this.LocLng == other.LocLng &&
                this.LocLat == other.LocLat &&
                this.CenterLng == other.CenterLng &&
                this.CenterLat == other.CenterLat &&
                this.MapZoom == other.MapZoom &&
                ((this.Author == null && other.Author == null) || this.Author.Equals(other.Author)) &&
                this.PhotoCategories.SequenceEqual(other.PhotoCategories)
            );
        }

        public override int GetHashCode() => Id;
    }
}