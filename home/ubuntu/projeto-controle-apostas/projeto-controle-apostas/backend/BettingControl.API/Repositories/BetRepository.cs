using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BettingControl.API.Data;
using BettingControl.API.Models;

namespace BettingControl.API.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly ApplicationDbContext _context;

        public BetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bet>> GetAllAsync()
        {
            return await _context.Bets.Include(b => b.Ciclo).ToListAsync();
        }

        public async Task<Bet> GetByIdAsync(int id)
        {
            return await _context.Bets.Include(b => b.Ciclo).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Bet bet)
        {
            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bet bet)
        {
            _context.Bets.Update(bet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bet bet)
        {
            _context.Bets.Remove(bet);
            await _context.SaveChangesAsync();
        }
    }
}

