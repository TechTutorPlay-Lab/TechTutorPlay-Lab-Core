using System;

namespace TechTutorPlay
{
    // Modello utente
    public class ApplicationUser
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? TwoFactorSecretKey { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}