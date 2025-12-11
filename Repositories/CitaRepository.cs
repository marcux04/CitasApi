using CitasApi.Data;
using CitasApi.DTOs;
using CitasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CitasApi.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDbContext _context;

        public CitaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> GetAllByPacienteAsync(int pacienteId)
        {
            return await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .Where(c => c.PacienteId == pacienteId)
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.Hora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetAllByMedicoAsync(int medicoId)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.MedicoId == medicoId)
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.Hora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetCitasFiltradasAsync(FiltroCitasDto filtro)
        {
            var query = _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .AsQueryable();

            if (filtro.PacienteId.HasValue)
                query = query.Where(c => c.PacienteId == filtro.PacienteId.Value);

            if (filtro.MedicoId.HasValue)
                query = query.Where(c => c.MedicoId == filtro.MedicoId.Value);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(c => c.Fecha >= filtro.FechaDesde.Value.Date);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(c => c.Fecha <= filtro.FechaHasta.Value.Date);

            if (!string.IsNullOrEmpty(filtro.Estado))
                query = query.Where(c => c.Estado == filtro.Estado);

            return await query
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.Hora)
                .ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cita> CreateAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task UpdateAsync(Cita cita)
        {
            _context.Entry(cita).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cita cita)
        {
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasOverlappingCitaAsync(int medicoId, DateTime fecha, TimeSpan hora, int? excludeCitaId = null)
        {
            var query = _context.Citas
                .Where(c => c.MedicoId == medicoId && 
                           c.Fecha == fecha && 
                           c.Hora == hora &&
                           c.Estado != "cancelada");

            if (excludeCitaId.HasValue)
                query = query.Where(c => c.Id != excludeCitaId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Citas.AnyAsync(c => c.Id == id);
        }
    }
}