using System.Collections.Generic;
using System.Threading.Tasks;
using BettingControl.API.Models;

namespace BettingControl.API.Repositories
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllAsync();
        Task<Bet> GetByIdAsync(int id);
        Task AddAsync(Bet bet);
        Task UpdateAsync(Bet bet);
        Task DeleteAsync(Bet bet);
    }
}

