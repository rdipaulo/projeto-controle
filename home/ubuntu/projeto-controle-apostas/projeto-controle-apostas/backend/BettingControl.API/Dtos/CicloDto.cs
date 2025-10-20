using BettingControl.API.Models;

namespace BettingControl.API.Dtos
{
    public class CicloDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalApostado { get; set; }
        public decimal TotalGanhos { get; set; }
        public decimal LucroPrejuizo { get; set; }
        public decimal Roi { get; set; }
        public bool IsClosed { get; set; }
        // Não incluir a lista de Bets aqui para evitar referência circular
        // public ICollection<BetDto> Bets { get; set; } // Se necessário, crie um BetDto
    }
}
