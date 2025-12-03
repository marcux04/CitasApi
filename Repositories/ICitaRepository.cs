using CitasApi.Models;

namespace CitasApi.Repositories
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> GetAllByPacienteAsync(int pacienteId);
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> CreateAsync(Cita cita);
        Task DeleteAsync(Cita cita);
        Task<bool> HasOverlappingCitaAsync(int medicoId, DateTime fecha, TimeSpan hora);
        Task<bool> ExistsAsync(int id);
    }
}