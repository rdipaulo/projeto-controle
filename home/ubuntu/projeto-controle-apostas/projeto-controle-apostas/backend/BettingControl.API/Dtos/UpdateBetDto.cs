using System.ComponentModel.DataAnnotations;
using BettingControl.API.Models;

namespace BettingControl.API.Dtos
{
    public class UpdateBetDto
    {
        [Required(ErrorMessage = "O país é obrigatório.")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "O continente é obrigatório.")]
        public string Continente { get; set; }

        [Required(ErrorMessage = "O campeonato é obrigatório.")]
        public string Campeonato { get; set; }

        [Required(ErrorMessage = "O time da casa é obrigatório.")]
        public string TimeCasa { get; set; }

        [Required(ErrorMessage = "O time visitante é obrigatório.")]
        public string TimeVisitante { get; set; }

        [Required(ErrorMessage = "O mercado é obrigatório.")]
        public string Mercado { get; set; }

        [Required(ErrorMessage = "A odd é obrigatória.")]
        [Range(1.0, double.MaxValue, ErrorMessage = "A odd deve ser maior que 1.")]
        public decimal Odd { get; set; }

        [Required(ErrorMessage = "O valor apostado é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor apostado deve ser maior que zero.")]
        public decimal ValorApostado { get; set; }

        [Required(ErrorMessage = "O resultado da aposta é obrigatório.")]
        public ResultadoAposta Resultado { get; set; }

        public int? CicloId { get; set; }
    }
}

