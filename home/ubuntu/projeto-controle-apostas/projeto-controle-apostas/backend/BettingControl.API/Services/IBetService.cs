using System.Collections.Generic;
using System.Threading.Tasks;
using BettingControl.API.Dtos;
using BettingControl.API.Models;

namespace BettingControl.API.Services
{
    public interface IBetService
    {
        Task<IEnumerable<Bet>> GetAllBetsAsync();
        Task<Bet> GetBetByIdAsync(int id);
        Task<Bet> AddBetAsync(CreateBetDto createBetDto);
        Task<Bet> UpdateBetAsync(int id, UpdateBetDto updateBetDto);
        Task<bool> DeleteBetAsync(int id);
    }
}

