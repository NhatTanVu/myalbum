using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Photo.API.Controllers.Resources
{
    public class AlbumResource : IEquatable<AlbumResource>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserResource Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IEnumerable<PhotoResource> Photos { get; set; }

        public AlbumResource()
        {
        }

        public bool Equals([AllowNull] AlbumResource other)
        {
            if (other == null)
                return false;

            return (
                this.Id == other.Id &&
                this.Name == other.Name
            );
        }
    }
}