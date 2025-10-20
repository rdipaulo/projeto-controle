using System.Threading.Tasks;
using BettingControl.API.Models;
using System.Collections.Generic;

namespace BettingControl.API.Services
{
    public interface IServicoFinanceiro
    {
        Task<decimal> CalcularROIGeralAsync(int userId);
        Task<decimal> CalcularROICicloAsync(int cicloId, int userId);
        Task<decimal> CalcularYieldAsync(int userId);
        Task<decimal> CalcularTaxaAcertoAsync(int userId);
        Task<decimal> CalcularLucroLiquidoAsync(int userId);
        Task<decimal> CalcularLucroAcumuladoAsync(int userId);
        Task RegistrarHistoricoBancaAsync(int userId, decimal saldo, string observacao);
        Task<IEnumerable<HistoricoBanca>> GetHistoricoBancaAsync(int userId);
    }
}

