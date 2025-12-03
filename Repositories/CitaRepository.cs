using CitasApi.Data;
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

        public async Task DeleteAsync(Cita cita)
        {
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasOverlappingCitaAsync(int medicoId, DateTime fecha, TimeSpan hora)
        {
            return await _context.Citas
                .AnyAsync(c => c.MedicoId == medicoId && 
                               c.Fecha == fecha && 
                               c.Hora == hora &&
                               c.Estado != "cancelada");
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Citas.AnyAsync(c => c.Id == id);
        }
    }
}