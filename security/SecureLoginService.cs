using System;

namespace TechTutorPlay
{
    public class SecureLoginService
    {
        private readonly PasswordHasher _passwordHasher;

        public SecureLoginService()
        {
            _passwordHasher = new PasswordHasher();
        }

        public bool Login(string username, string password)
        {
            // Implementa la logica di login
            Console.WriteLine($"Tentativo di login per: {username}");
            
            // Esempio: verifica con password hasciata
            // string storedHash = GetStoredPasswordHash(username);
            // return _passwordHasher.VerifyPassword(password, storedHash);
            
            return false;
        }

        public void LogLoginAttempt(string username, bool success)
        {
            string status = success ? "SUCCESS" : "FAILED";
            Console.WriteLine($"[{DateTime.Now}] Login attempt for {username}: {status}");
        }
    }
}