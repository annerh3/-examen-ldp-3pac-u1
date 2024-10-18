using Microsoft.AspNetCore.Identity;

namespace ProyectoExamenU1.Helpers.Users
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } // Navegación a los roles
    }
}
