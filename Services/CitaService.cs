using CitasApi.DTOs;
using CitasApi.Models;
using CitasApi.Repositories;

namespace CitasApi.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;

        public CitaService(
            ICitaRepository citaRepository,
            IPacienteRepository pacienteRepository,
            IMedicoRepository medicoRepository)
        {
            _citaRepository = citaRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task<Cita> CreateCitaAsync(CitaCreateDto citaDto)
        {
            // Validar que el paciente existe
            var paciente = await _pacienteRepository.GetByIdAsync(citaDto.PacienteId);
            if (paciente == null)
                throw new KeyNotFoundException("Paciente no encontrado");

            // Validar que el médico existe
            var medico = await _medicoRepository.GetByIdAsync(citaDto.MedicoId);
            if (medico == null)
                throw new KeyNotFoundException("Médico no encontrado");

            // Validar que la fecha sea futura
            var fechaDate = citaDto.Fecha.Date;
            var hoy = DateTime.Today;
            
            if (fechaDate <= hoy)
                throw new InvalidOperationException("La fecha debe ser futura");

            // Validar solapamiento de citas
            if (await _citaRepository.HasOverlappingCitaAsync(
                citaDto.MedicoId, fechaDate, citaDto.Hora))
            {
                throw new InvalidOperationException("El médico ya tiene una cita en esa fecha y hora");
            }

            var cita = new Cita
            {
                PacienteId = citaDto.PacienteId,
                MedicoId = citaDto.MedicoId,
                Fecha = fechaDate,
                Hora = citaDto.Hora,
                Estado = "pendiente"
            };

            return await _citaRepository.CreateAsync(cita);
        }

        public async Task<IEnumerable<Cita>> GetCitasByPacienteAsync(int pacienteId)
        {
            // Validar que el paciente existe
            var paciente = await _pacienteRepository.GetByIdAsync(pacienteId);
            if (paciente == null)
                throw new KeyNotFoundException("Paciente no encontrado");

            return await _citaRepository.GetAllByPacienteAsync(pacienteId);
        }

        public async Task<bool> DeleteCitaAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null)
                return false;

            await _citaRepository.DeleteAsync(cita);
            return true;
        }
    }
}