using Microsoft.AspNetCore.Identity;

namespace P127ProniaProject.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
