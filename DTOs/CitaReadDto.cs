namespace CitasApi.DTOs
{
    public class CitaReadDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = null!;
        public int MedicoId { get; set; }
        public string MedicoNombre { get; set; } = null!;
        public string Especialidad { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; } = null!;
    }
}