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

            // Validar horario laboral (8:00 - 20:00)
            if (citaDto.Hora < TimeSpan.FromHours(8) || citaDto.Hora > TimeSpan.FromHours(20))
                throw new InvalidOperationException("El horario debe estar entre 8:00 y 20:00");

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

        public async Task<Cita?> GetCitaByIdAsync(int id)
        {
            return await _citaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cita>> GetCitasByPacienteAsync(int pacienteId)
        {
            // Validar que el paciente existe
            var paciente = await _pacienteRepository.GetByIdAsync(pacienteId);
            if (paciente == null)
                throw new KeyNotFoundException("Paciente no encontrado");

            return await _citaRepository.GetAllByPacienteAsync(pacienteId);
        }

        public async Task<IEnumerable<Cita>> GetCitasByMedicoAsync(int medicoId)
        {
            // Validar que el médico existe
            var medico = await _medicoRepository.GetByIdAsync(medicoId);
            if (medico == null)
                throw new KeyNotFoundException("Médico no encontrado");

            return await _citaRepository.GetAllByMedicoAsync(medicoId);
        }

        public async Task<IEnumerable<Cita>> GetCitasFiltradasAsync(FiltroCitasDto filtro)
        {
            return await _citaRepository.GetCitasFiltradasAsync(filtro);
        }

        public async Task<Cita?> UpdateCitaAsync(int id, CitaUpdateDto citaDto)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null)
                return null;

            if (citaDto.MedicoId.HasValue)
            {
                var medico = await _medicoRepository.GetByIdAsync(citaDto.MedicoId.Value);
                if (medico == null)
                    throw new KeyNotFoundException("Médico no encontrado");
                cita.MedicoId = citaDto.MedicoId.Value;
            }

            if (citaDto.Fecha.HasValue)
            {
                var fechaDate = citaDto.Fecha.Value.Date;
                var hoy = DateTime.Today;
                
                if (fechaDate <= hoy)
                    throw new InvalidOperationException("La fecha debe ser futura");
                
                cita.Fecha = fechaDate;
            }

            if (citaDto.Hora.HasValue)
            {
                // Validar horario laboral
                if (citaDto.Hora.Value < TimeSpan.FromHours(8) || citaDto.Hora.Value > TimeSpan.FromHours(20))
                    throw new InvalidOperationException("El horario debe estar entre 8:00 y 20:00");
                
                cita.Hora = citaDto.Hora.Value;
            }

            // Validar solapamiento si cambió médico, fecha o hora
            if (citaDto.MedicoId.HasValue || citaDto.Fecha.HasValue || citaDto.Hora.HasValue)
            {
                var medicoId = citaDto.MedicoId ?? cita.MedicoId;
                var fecha = citaDto.Fecha ?? cita.Fecha;
                var hora = citaDto.Hora ?? cita.Hora;

                if (await _citaRepository.HasOverlappingCitaAsync(medicoId, fecha, hora, id))
                    throw new InvalidOperationException("El médico ya tiene una cita en esa fecha y hora");
            }

            await _citaRepository.UpdateAsync(cita);
            return cita;
        }

        public async Task<Cita?> CambiarEstadoCitaAsync(int id, string estado)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null)
                return null;

            var estadosPermitidos = new[] { "pendiente", "confirmada", "cancelada", "completada" };
            if (!estadosPermitidos.Contains(estado.ToLower()))
                throw new InvalidOperationException($"Estado no válido. Estados permitidos: {string.Join(", ", estadosPermitidos)}");

            cita.Estado = estado.ToLower();
            await _citaRepository.UpdateAsync(cita);
            return cita;
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