using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoExamenU1.Database.Entities
{
    [Table("permition_application", Schema = "dbo")]
    public class PermitionApplicationEntity : BaseEntity
    {
        [Display(Name = "start_date")]
        [MinLength(10, ErrorMessage = "La {0} debe ser valida")]
        [Column("end_date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha de Finalizacion")]
        [MinLength(10, ErrorMessage = "La {0} debe ser valida")]
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Descripción")]
        [MinLength(10, ErrorMessage = "La {0} debe tener al menos {1} caracteres.")]
        [StringLength(250)]
        [Column("reason")]
        public string Reason { get; set; }

        [Column("permition_type_id")]
        [ForeignKey(nameof(PermitionTypeID))]
        public virtual PermitionTypeEntity PermitionTypeID { get; set; }

        [Column("state")]
        public string State { get; set; }

        public virtual IdentityUser CreateByUser { get; set; }
        public virtual IdentityUser UpdateByUser { get; set; }
    }
}
