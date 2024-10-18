using ProyectoExamenU1.Services.Interfaces;

namespace ProyectoExamenU1.Services
{
    public class AuditService : IAuditService
    {
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            // si no hay http context es el seeder 
            if (_httpContextAccessor.HttpContext == null)
            {
                
                return "Seeder";
            }
            // para normal
            var idClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (idClaim == null)
            {
                throw new InvalidOperationException("No UserId claim present");
            }

            return idClaim.Value;
        }
    }
}
