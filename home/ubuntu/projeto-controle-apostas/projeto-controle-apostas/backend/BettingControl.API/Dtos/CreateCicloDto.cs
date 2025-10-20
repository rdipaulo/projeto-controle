using System.ComponentModel.DataAnnotations;

namespace BettingControl.API.Dtos
{
    public class CreateCicloDto
    {
        [Required(ErrorMessage = "O nome do ciclo é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime StartDate { get; set; }
        
    }
}