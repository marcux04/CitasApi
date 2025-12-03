using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Services
{
    public interface ICitaService
    {
        Task<Cita> CreateCitaAsync(CitaCreateDto citaDto);
        Task<IEnumerable<Cita>> GetCitasByPacienteAsync(int pacienteId);
        Task<bool> DeleteCitaAsync(int id);
    }
}