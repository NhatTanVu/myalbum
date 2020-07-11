using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Core.Models
{
    public class Photo : IEquatable<Photo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string FilePath { get; set; }
        public double? LocLng { get; set; }
        public double? LocLat { get; set; }
        public double? CenterLng { get; set; }
        public double? CenterLat { get; set; }
        public int? MapZoom { get; set; }
        public string MapFilePath { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PhotoCategory> PhotoCategories { get; set; }

        public Album Album { get; set; }

        public User Author { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Photo()
        {
            Comments = new Collection<Comment>();
            PhotoCategories = new Collection<PhotoCategory>();
        }

        public bool Equals([AllowNull] Photo other)
        {
            return (
                this.Id == other.Id &&
                this.Name == other.Name &&
                this.Width == other.Width &&
                this.Height == other.Height &&
                this.FilePath == other.FilePath &&
                this.LocLng == other.LocLng &&
                this.LocLat == other.LocLat &&
                this.CenterLng == other.CenterLng &&
                this.CenterLat == other.CenterLat &&
                this.MapZoom  == other.MapZoom
            );
        }

        public override int GetHashCode() => Id;
    }
}