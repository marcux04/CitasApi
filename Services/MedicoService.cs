using CitasApi.Models;
using CitasApi.Repositories;

namespace CitasApi.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _repository;

        public MedicoService(IMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Medico>> GetAllMedicosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Medico?> GetMedicoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}