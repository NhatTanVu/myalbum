using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Album.API.Controllers.Resources
{
    public class AddAlbumResource : IEquatable<AddAlbumResource>
    {
        public string Name { get; set; }

        public AddAlbumResource()
        {
        }

        public bool Equals([AllowNull] AddAlbumResource other)
        {
            if (other == null)
                return false;

            return (
                this.Name == other.Name
            );
        }
    }
}