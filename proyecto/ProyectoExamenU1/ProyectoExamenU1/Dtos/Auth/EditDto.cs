namespace ProyectoExamenU1.Dtos.Auth
{
    public class EditDto
    {
        public string EmployeeName { get; set; }
        public string Email { get; set; }        
        public string Password { get; set; }        
        public Guid Cargo { get; set; }     // Id del rol
        public DateOnly FechaIngreso { get; set; }  // Fecha de ingreso del empleado

    }
}
