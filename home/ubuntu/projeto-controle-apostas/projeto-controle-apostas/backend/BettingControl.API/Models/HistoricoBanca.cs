using System;
using System.ComponentModel.DataAnnotations;

namespace BettingControl.API.Models
{
    public class HistoricoBanca
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal Saldo { get; set; }
        public string Observacao { get; set; } // Para registrar depósitos/saques
    }
}

