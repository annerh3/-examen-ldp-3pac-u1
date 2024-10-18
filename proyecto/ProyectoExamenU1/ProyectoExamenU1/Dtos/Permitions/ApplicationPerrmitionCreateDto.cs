using ProyectoExamenU1.Database.Entities;

namespace ProyectoExamenU1.Dtos.Permitions
{
    public class ApplicationPerrmitionCreateDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public Guid PermitionTypeId { get; set; }

        public string State { get; set; }
    }
}
