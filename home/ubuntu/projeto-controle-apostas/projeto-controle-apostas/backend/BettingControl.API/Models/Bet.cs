using System;
using System.ComponentModel.DataAnnotations;
using BettingControl.API.Models;

namespace BettingControl.API.Models
{
    public enum ResultadoAposta
    {
        Pendente,
        Ganha,
        Perdida,
        Cancelada
    }

    public class Bet
    {
        public int Id { get; set; }
        public string Pais { get; set; }
        public string Continente { get; set; }
        public string Campeonato { get; set; }
        public string TimeCasa { get; set; }
        public string TimeVisitante { get; set; }
        public string Mercado { get; set; }
        public decimal Odd { get; set; }
        public decimal ValorApostado { get; set; }
        public ResultadoAposta Resultado { get; set; }
        public DateTime DataAposta { get; set; }

        // Relacionamento com Ciclo (opcional)
        public int? CicloId { get; set; }
        public Ciclo Ciclo { get; set; }

        // Propriedades para cálculos financeiros
        public decimal LucroPrejuizo { get; set; } // Calculado com base no Resultado
    }
}

