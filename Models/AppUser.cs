using Microsoft.AspNetCore.Identity;

namespace InsuranceWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}