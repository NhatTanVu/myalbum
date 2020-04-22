using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace MyAlbum.WebSPA.Controllers.Resources
{
    public class PhotoResource: IEquatable<PhotoResource>
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
        public string MapFilePath { get; set; }        
        public ICollection<CommentResource> Comments { get; set; }
        public ICollection<CategoryResource> PhotoCategories { get; set; }
        public AlbumResource Album { get; set; }
        public UserResource Author { get; set; }

        public PhotoResource()
        {
            Comments = new Collection<CommentResource>();
            PhotoCategories = new Collection<CategoryResource>();
        }

        public bool Equals([AllowNull] PhotoResource other)
        {
            return (this.Id == other.Id);
        }

        public override int GetHashCode() => Id;
    }
}