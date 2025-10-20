using System.Threading.Tasks;
using BettingControl.API.Models;

namespace BettingControl.API.Services
{
    public interface IServicoDeFechamentoMensal
    {
        Task<FechamentoMensal> RealizarFechamentoMensalAsync(int userId, int ano, int mes);
        Task<FechamentoMensal> GetFechamentoMensalAsync(int userId, int ano, int mes);
    }
}

