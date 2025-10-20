using BettingControl.API.Models;

namespace BettingControl.API.Repositories
{
    public interface ICicloRepository
    {
        Task<Ciclo?> GetByIdAsync(int id);
        Task<IEnumerable<Ciclo>> GetAllByUserIdAsync(int userId);
        Task AddAsync(Ciclo ciclo);
        Task UpdateAsync(Ciclo ciclo);
        Task DeleteAsync(Ciclo ciclo);
    }
}