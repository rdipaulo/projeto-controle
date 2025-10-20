using BettingControl.API.Dtos;
using BettingControl.API.Models;

namespace BettingControl.API.Services
{
    public interface ICicloService
    {
        Task<IEnumerable<Ciclo>> GetUserCiclosAsync(int userId);
        Task<Ciclo?> GetCicloByIdAsync(int id, int userId);
        Task<Ciclo> CreateCicloAsync(CreateCicloDto dto, int userId);
        Task<bool> UpdateCicloAsync(int id, UpdateCicloDto dto, int userId);
        Task<bool> DeleteCicloAsync(int id, int userId);
        Task<CicloDto?> EncerrarCicloAsync(int id, int userId);
    }
}

