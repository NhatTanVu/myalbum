using System;

namespace MyAlbum.WebSPA.Controllers.Resources
{
    public class UserResource
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}