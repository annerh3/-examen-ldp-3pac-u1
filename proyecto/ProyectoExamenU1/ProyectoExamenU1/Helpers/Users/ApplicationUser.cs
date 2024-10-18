using Microsoft.AspNetCore.Identity;

namespace ProyectoExamenU1.Helpers.Users
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } // Navegación a los roles del usuario
    }
}
