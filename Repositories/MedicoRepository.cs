using CitasApi.Data;
using CitasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CitasApi.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly AppDbContext _context;

        public MedicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await _context.Medicos.ToListAsync();
        }

        public async Task<Medico?> GetByIdAsync(int id)
        {
            return await _context.Medicos.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Medicos.AnyAsync(m => m.Id == id);
        }
    }
}