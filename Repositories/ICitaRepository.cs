using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Repositories
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> GetAllByPacienteAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetAllByMedicoAsync(int medicoId);
        Task<IEnumerable<Cita>> GetCitasFiltradasAsync(FiltroCitasDto filtro);
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> CreateAsync(Cita cita);
        Task UpdateAsync(Cita cita);
        Task DeleteAsync(Cita cita);
        Task<bool> HasOverlappingCitaAsync(int medicoId, DateTime fecha, TimeSpan hora, int? excludeCitaId = null);
        Task<bool> ExistsAsync(int id);
    }
}