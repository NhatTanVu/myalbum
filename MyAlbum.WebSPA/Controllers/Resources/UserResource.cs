using System;
using System.Diagnostics.CodeAnalysis;

namespace MyAlbum.WebSPA.Controllers.Resources
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
            return (
                this.UserName == other.UserName
            );
        }

        public override int GetHashCode() => UserName.GetHashCode();
    }
}