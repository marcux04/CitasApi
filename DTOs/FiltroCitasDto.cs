namespace CitasApi.DTOs
{
    public class FiltroCitasDto
    {
        public int? PacienteId { get; set; }
        public int? MedicoId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? Estado { get; set; }
    }
}