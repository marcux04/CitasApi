using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Services
{
    public interface ICitaService
    {
        Task<Cita> CreateCitaAsync(CitaCreateDto citaDto);
        Task<Cita?> GetCitaByIdAsync(int id);
        Task<IEnumerable<Cita>> GetCitasByPacienteAsync(int pacienteId);
        Task<IEnumerable<Cita>> GetCitasByMedicoAsync(int medicoId);
        Task<IEnumerable<Cita>> GetCitasFiltradasAsync(FiltroCitasDto filtro);
        Task<Cita?> UpdateCitaAsync(int id, CitaUpdateDto citaDto);
        Task<Cita?> CambiarEstadoCitaAsync(int id, string estado);
        Task<bool> DeleteCitaAsync(int id);
    }
}