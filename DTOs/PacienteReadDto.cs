namespace CitasApi.DTOs
{
    public class PacienteReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Curp { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Correo { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}