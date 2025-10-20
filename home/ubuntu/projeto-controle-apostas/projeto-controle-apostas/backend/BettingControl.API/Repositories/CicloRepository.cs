using BettingControl.API.Data;
using BettingControl.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingControl.API.Repositories
{
    public class CicloRepository : ICicloRepository
    {
        private readonly ApplicationDbContext _context;

        public CicloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ciclo?> GetByIdAsync(int id)
        {
            return await _context.Ciclos
                .Include(c => c.Bets)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Ciclo>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Ciclos
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }

        public async Task AddAsync(Ciclo ciclo)
        {
            _context.Ciclos.Add(ciclo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ciclo ciclo)
        {
            _context.Ciclos.Update(ciclo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ciclo ciclo)
        {
            _context.Ciclos.Remove(ciclo);
            await _context.SaveChangesAsync();
        }
    }
}