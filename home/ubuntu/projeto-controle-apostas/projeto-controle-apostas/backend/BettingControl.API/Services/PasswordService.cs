using BCrypt.Net;

namespace BettingControl.API.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly string _pepper;

        public PasswordService(string pepper)
        {
            _pepper = pepper ?? string.Empty;
        }

        public string HashPassword(string password)
        {
            // Combine password with a server-side pepper before hashing
            var toHash = password + _pepper;
            return BCrypt.Net.BCrypt.EnhancedHashPassword(toHash);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var toVerify = password + _pepper;
            return BCrypt.Net.BCrypt.EnhancedVerify(toVerify, hashedPassword);
        }
    }
}
