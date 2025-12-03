using CitasApi.Models;

namespace CitasApi.Services
{
    public interface IMedicoService
    {
        Task<IEnumerable<Medico>> GetAllMedicosAsync();
        Task<Medico?> GetMedicoByIdAsync(int id);
    }
}