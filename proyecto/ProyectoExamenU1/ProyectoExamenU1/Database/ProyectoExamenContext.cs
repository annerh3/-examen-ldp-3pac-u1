using Microsoft.EntityFrameworkCore;

namespace ProyectoExamenU1.Database
{
    public class ProyectoExamenContext : DbContext
    {

        public ProyectoExamenContext(DbContextOptions options) : base(options)
        {
            
        }

        //public DbSet<ENTITY_CLASS> ENTITY_NAME { get; set; }
    }
}
