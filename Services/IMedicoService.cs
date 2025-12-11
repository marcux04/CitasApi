using CitasApi.DTOs;
using CitasApi.Models;

namespace CitasApi.Services
{
    public interface IMedicoService
    {
        Task<IEnumerable<Medico>> GetAllMedicosAsync();
        Task<Medico?> GetMedicoByIdAsync(int id);
        Task<Medico> CreateMedicoAsync(MedicoCreateDto medicoDto);
        Task<Medico?> UpdateMedicoAsync(int id, MedicoUpdateDto medicoDto);
        Task<bool> DeleteMedicoAsync(int id);
    }
}