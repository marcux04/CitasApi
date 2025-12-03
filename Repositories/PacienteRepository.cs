using CitasApi.Data;
using CitasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CitasApi.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;

        public PacienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        public async Task<Paciente?> GetByIdAsync(int id)
        {
            return await _context.Pacientes.FindAsync(id);
        }

        public async Task<Paciente?> GetByEmailAsync(string email)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Correo == email);
        }

        public async Task<Paciente> CreateAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Paciente paciente)
        {
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByCurpAsync(string curp)
        {
            return await _context.Pacientes.AnyAsync(p => p.Curp == curp);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Pacientes.AnyAsync(p => p.Correo == email);
        }
    }
}