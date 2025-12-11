using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Services
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllPacientesAsync();
        Task<IEnumerable<Paciente>> GetPacientesFiltradosAsync(FiltroPacientesDto filtro);
        Task<Paciente> CreatePacienteAsync(PacienteCreateDto pacienteDto);
        Task<Paciente?> GetPacienteByIdAsync(int id);
        Task<Paciente?> UpdatePacienteAsync(int id, PacienteUpdateDto pacienteDto);
        Task<bool> DeletePacienteAsync(int id);
        Task<Paciente?> AuthenticateAsync(string email, string password);
        Task<Paciente?> CambiarRolAsync(int id, string nuevoRol);
    }
}