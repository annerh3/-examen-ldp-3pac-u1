using Microsoft.AspNetCore.Identity;
using ProyectoExamenU1.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoExamenU1.Dtos.Permitions
{
    public class ApplicationPermitionDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public Guid PermitionTypeId { get; set; }

        public string State { get; set; }

    }
}
