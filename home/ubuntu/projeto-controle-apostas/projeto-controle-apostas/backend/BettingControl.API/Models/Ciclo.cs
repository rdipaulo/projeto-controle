using System.Collections.Generic; // Garante que ICollection e List funcionem

namespace BettingControl.API.Models
{
    public class Ciclo // Nome da classe agora é Ciclo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalApostado { get; set; }
        public decimal TotalGanhos { get; set; }
        public decimal LucroPrejuizo { get; set; }
        public decimal ROI { get; set; }
        public int UserId { get; set; }
        
        // Propriedade de Navegação
        public User? User { get; set; }
        public ICollection<Bet> Bets { get; set; } = new List<Bet>();
        

        // Propriedades para controle de status do ciclo
        public bool IsClosed { get; set; } = false; // Indica se o ciclo foi encerrado
    }
}