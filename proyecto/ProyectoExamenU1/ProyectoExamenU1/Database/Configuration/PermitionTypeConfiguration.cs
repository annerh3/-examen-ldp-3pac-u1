using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoExamenU1.Database.Entities;

namespace ProyectoExamenU1.Database.Configuration
{
    public class PermitionTypeConfiguration : IEntityTypeConfiguration<PermitionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<PermitionTypeEntity> builder)
        {
            builder.HasOne(e => e.CreateByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .HasPrincipalKey(e => e.Id);
            //.IsRequired();
            builder.HasOne(e => e.UpdateByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .HasPrincipalKey(e => e.Id);
               // .IsRequired();
        }
    }
}
