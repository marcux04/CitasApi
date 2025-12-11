using CitasApi.Models;

namespace CitasApi.Repositories
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico?> GetByIdAsync(int id);
        Task<Medico> CreateAsync(Medico medico);
        Task UpdateAsync(Medico medico);
        Task DeleteAsync(Medico medico);
        Task<bool> ExistsAsync(int id);
    }
}