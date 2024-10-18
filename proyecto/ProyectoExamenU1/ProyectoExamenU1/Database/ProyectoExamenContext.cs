using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProyectoExamenU1.Database
{
    public class ProyectoExamenContext : DbContext
    {

        public ProyectoExamenContext(
            DbContextOptions options
  
            ) 
            : base(options)
        {
    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");


            //Le decimos que nuestras tablas se crearan en esquema de security
            modelBuilder.HasDefaultSchema("security");

            //ASignamos nombres a nuestras tablas para no confundirnos
            modelBuilder.Entity<IdentityUser>().ToTable("users");
            modelBuilder.Entity<IdentityRole>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("users_roles");

            //Estos son los permisos
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("users_logins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");

           // modelBuilder.ApplyConfiguration(new CategoryConfiguration());          <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            


            //set FKs on Restrict
            var eTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var type in eTypes)
            {
                var foreingkeys = type.GetForeignKeys();
                foreach (var foreingkey in foreingkeys)
                {
                    foreingkey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }



        //public DbSet<ENTITY_CLASS> ENTITY_NAME { get; set; }
    }
}
