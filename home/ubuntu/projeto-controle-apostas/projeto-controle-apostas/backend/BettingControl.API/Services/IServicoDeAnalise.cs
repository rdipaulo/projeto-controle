using System.Collections.Generic;
using System.Threading.Tasks;
using BettingControl.API.Models;

namespace BettingControl.API.Services
{
    public interface IServicoDeAnalise
    {
        Task<IEnumerable<string>> AnalisarMercadosLucrativosDeficitariosAsync(int userId);
        Task<IEnumerable<string>> AnalisarCampeonatosLucrativosDeficitariosAsync(int userId);
        Task<IEnumerable<string>> AnalisarPaisesLucrativosDeficitariosAsync(int userId);
        Task<IEnumerable<string>> AnalisarTimesLucrativosDeficitariosAsync(int userId);
        Task<IEnumerable<string>> EmitirAlertasInteligentesAsync(int userId);
        Task<IEnumerable<AnaliseMercadoDto>> GetDadosAnaliseMercadoAsync(int userId);
    }

    public class AnaliseMercadoDto
    {
        public string Nome { get; set; }
        public decimal LucroPrejuizo { get; set; }
        public decimal ROI { get; set; }
        public int TotalApostas { get; set; }
        public int ApostasGanhas { get; set; }
        public int ApostasPerdidas { get; set; }
        public decimal TotalApostado { get; set; }
    }
}

