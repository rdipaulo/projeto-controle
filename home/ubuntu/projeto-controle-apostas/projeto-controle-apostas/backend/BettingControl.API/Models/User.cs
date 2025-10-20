namespace BettingControl.API.Models
{
    public class User
    {
        public int Id { get; set; }

        // Mudei de 'Username' para 'Name' para compatibilidade com RegisterDto
        public string Name { get; set; } = string.Empty; 
        
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; } = string.Empty;
        
        // Propriedades de Timestamp (essenciais para o AuthController)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
