using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.WebSPA.Controllers.Resources
{
    public class CategoryResource : IEquatable<CategoryResource>
    {
        public int Id {get; set; }
        public string Name { get; set; }
        
        public bool Equals([AllowNull] CategoryResource other)
        {
            return (
                this.Name == other.Name
            );
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}