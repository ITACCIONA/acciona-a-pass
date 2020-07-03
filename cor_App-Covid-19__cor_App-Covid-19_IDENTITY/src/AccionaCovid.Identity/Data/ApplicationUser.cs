using Microsoft.AspNetCore.Identity;
using System;

namespace AccionaCovid.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int? IdEmpleado { get; set; }

        public DateTimeOffset? PasswordExpiration { get; set; }
    }
}
