using Microsoft.AspNetCore.Identity;

namespace StudentWebAPI.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
