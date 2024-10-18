using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProyectoExamenU1.Database.Entities
{
    [Table("permition_type", Schema = "dbo")]
    public class PermitionTypeEntity:BaseEntity
    {
        [StringLength(100)]
        [Required]
        [Column("type")]
        public string Type { get; set; }
        public virtual IdentityUser CreateByUser { get; set; }
        public virtual IdentityUser UpdateByUser { get; set; }
    }
}

