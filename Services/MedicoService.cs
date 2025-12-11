using CitasApi.DTOs;
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

        public async Task<Medico> CreateMedicoAsync(MedicoCreateDto medicoDto)
        {
            var medico = new Medico
            {
                Nombre = medicoDto.Nombre,
                Especialidad = medicoDto.Especialidad
            };

            return await _repository.CreateAsync(medico);
        }

        public async Task<Medico?> UpdateMedicoAsync(int id, MedicoUpdateDto medicoDto)
        {
            var medico = await _repository.GetByIdAsync(id);
            if (medico == null)
                return null;

            if (!string.IsNullOrEmpty(medicoDto.Nombre))
                medico.Nombre = medicoDto.Nombre;

            if (!string.IsNullOrEmpty(medicoDto.Especialidad))
                medico.Especialidad = medicoDto.Especialidad;

            await _repository.UpdateAsync(medico);
            return medico;
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            var medico = await _repository.GetByIdAsync(id);
            if (medico == null)
                return false;

            await _repository.DeleteAsync(medico);
            return true;
        }
    }
}