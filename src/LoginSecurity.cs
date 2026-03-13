using System;

namespace TechTutorPlay.Security
{
    /// <summary>
    /// Gestisce la logica di accesso simulata per il portale.
    /// </summary>
    public class LoginSecurity
    {
        private readonly string _usernameCorretto;
        private readonly int _pinSicurezza;

        public LoginSecurity(string username = "admin@techtutorplay.tech", int pin = 2026)
        {
            _usernameCorretto = username;
            _pinSicurezza = pin;
        }

        public bool Authenticate()
        {
            Console.WriteLine("=== TechTutorPlay: Sistema di Accesso Protetto ===");
            Console.Write("Inserire il PIN di accesso per l'amministratore: ");

            string? inputUtente = Console.ReadLine();

            if (inputUtente == _pinSicurezza.ToString())
            {
                Console.WriteLine("\n[SUCCESS] Accesso autorizzato a TechTutorPlay Lab.");
                Console.WriteLine($"Benvenuto, {_usernameCorretto}!");
                return true;
            }
            else
            {
                Console.WriteLine("\n[ERROR] PIN errato. Tentativo di accesso registrato.");
                return false;
            }
        }

        public static void RunStandalone()
        {
            var login = new LoginSecurity();
            login.Authenticate();

            Console.WriteLine("\nPremere un tasto per terminare la sessione di test.");
            Console.ReadKey();
        }
    }
}
