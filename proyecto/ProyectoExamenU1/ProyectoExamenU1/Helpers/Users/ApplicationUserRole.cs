using Microsoft.AspNetCore.Identity;
using ProyectoExamenU1.Helpers.Users;

namespace ProyectoExamenU1.Helpers
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; } 
        public virtual ApplicationRole Role { get; set; }
    }
}
