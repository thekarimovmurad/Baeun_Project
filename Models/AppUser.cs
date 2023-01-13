using Microsoft.AspNetCore.Identity;

namespace Baeun_Project.Models
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
    }
}
