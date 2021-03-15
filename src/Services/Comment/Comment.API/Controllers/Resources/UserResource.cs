using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.Services.Comment.API.Controllers.Resources
{
    public class UserResource : IEquatable<UserResource>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool Equals([AllowNull] UserResource other)
        {
            if (other == null)
                return false;

            return (
                this.UserName == other.UserName
            );
        }

        public override int GetHashCode() => UserName.GetHashCode();
    }
}