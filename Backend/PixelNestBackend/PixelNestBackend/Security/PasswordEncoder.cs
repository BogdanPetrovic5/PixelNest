using Microsoft.AspNetCore.Identity;
namespace CarWebShop.Security
{
    public class PasswordEncoder
    {
        private readonly PasswordHasher<string> _passwordHasher;
        public PasswordEncoder(PasswordHasher<string> passwordHasher) {
            _passwordHasher = passwordHasher;
        }
        public string EncodePassword(string password)
        {
    
            return _passwordHasher.HashPassword(null, password);
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
