using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Album.API.Controllers.Resources
{
    public class CategoryResource : IEquatable<CategoryResource>
    {
        public int Id {get; set; }
        public string Name { get; set; }
        
        public bool Equals([AllowNull] CategoryResource other)
        {
            if (other == null)
                return false;

            return (
                this.Name == other.Name
            );
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}