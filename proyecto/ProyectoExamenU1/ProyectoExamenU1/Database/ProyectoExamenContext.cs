using BlogUNAH.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoExamenU1.Database.Configuration;
using ProyectoExamenU1.Database.Entities;
using ProyectoExamenU1.Services.Interfaces;

namespace ProyectoExamenU1.Database
{
    public class ProyectoExamenContext : IdentityDbContext<IdentityUser>
    {
        private readonly IAuditService _auditService;

        public ProyectoExamenContext(
            DbContextOptions options,
            IAuditService auditService   
  
            ) 
            : base(options)
        {
            this._auditService = auditService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<PermitionTypeEntity>()   // por que usa una entidad
            .Property(e => e.Type)   // y por que el nombre
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");



            //Le decimos que nuestras tablas se crearan en esquema de security
            modelBuilder.HasDefaultSchema("security");

            //ASignamos nombres a nuestras tablas para no confundirnos
            modelBuilder.Entity<Employee>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("AspNetUsers");

                entityTypeBuilder.Property(u => u.employee_name)
                    .HasMaxLength(100)
                    .HasDefaultValue(0);

                entityTypeBuilder.Property(u => u.date_entry)
                    .HasMaxLength(100);


            });
            modelBuilder.Entity<IdentityRole>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("users_roles");

            //Estos son los permisos
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("users_logins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");

            // modelBuilder.ApplyConfiguration(new CategoryConfiguration());          <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            modelBuilder.ApplyConfiguration(new PermitionApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new PermitionTypeConfiguration());


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
        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified
                ));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;
                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = _auditService.GetUserId();
                        entity.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        entity.UpdatedBy = _auditService.GetUserId();
                        entity.UpdatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        //public DbSet<ENTITY_CLASS> ENTITY_NAME { get; set; }
        public DbSet<PermitionApplicationEntity> ApplicationEntities { get; set; }
        public DbSet<PermitionTypeEntity> PermitionTypes { get; set; }

    }
}
