using System;
using System.ComponentModel.DataAnnotations;

namespace BettingControl.API.Models
{
    public class FechamentoMensal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime MesReferencia { get; set; } // O primeiro dia do mês de referência
        public decimal TotalApostado { get; set; }
        public decimal TotalGanhos { get; set; }
        public decimal LucroPrejuizo { get; set; }
        public decimal ROI { get; set; }
        public decimal Yield { get; set; }
        public decimal TaxaAcerto { get; set; }
        public string SugestoesAnalise { get; set; } // Sugestões do agente analista
    }
}

