using Microsoft.AspNetCore.Identity;

namespace BlogData.Entities
{
    public class BlogOwner : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
