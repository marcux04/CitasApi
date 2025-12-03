using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Services
{
    public interface IPacienteService
    {
        Task<Paciente> CreatePacienteAsync(PacienteCreateDto pacienteDto);
        Task<Paciente?> GetPacienteByIdAsync(int id);
        Task<Paciente?> UpdatePacienteAsync(int id, PacienteUpdateDto pacienteDto);
        Task<bool> DeletePacienteAsync(int id);
        Task<Paciente?> AuthenticateAsync(string email, string password);
    }
}