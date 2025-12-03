using CitasApi.Models;

namespace CitasApi.Repositories
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(int id);
        Task<Paciente?> GetByEmailAsync(string email);
        Task<Paciente> CreateAsync(Paciente paciente);
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(Paciente paciente);
        Task<bool> ExistsByCurpAsync(string curp);
        Task<bool> ExistsByEmailAsync(string email);
    }
}