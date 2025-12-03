using CitasApi.Models;

namespace CitasApi.Repositories
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}