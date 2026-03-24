using System; // Importa le funzioni base del sistema (come scrivere in console)
using System.Runtime.InteropServices; // Serve per capire su che sistema operativo stiamo girando

namespace TechTutorPlayLab.Core // Organizza il codice sotto il nome del tuo Lab
{
    class SystemCheck // Definisce la classe principale del programma
    {
        static void Main(string[] args) // Il punto di inizio (entry point) dello script
        {
            // Stampiamo un'intestazione professionale in "Google Blue style" (simulato)
            Console.WriteLine("========================================");
            Console.WriteLine("   TECHTUTORPLAY LAB - SYSTEM CHECK    ");
            Console.WriteLine("   License: MIT | Author: D. Vendramin ");
            Console.WriteLine("========================================\n");

            // Recupera il nome del PC e dell'utente attuale
            string machineName = Environment.MachineName;
            string userName = Environment.UserName;

            // Mostra un messaggio di benvenuto personalizzato
            Console.WriteLine($"Benvenuto nel Lab, {userName}!");
            Console.WriteLine($"Esecuzione su workstation: {machineName}");

            // Controlla se il sistema operativo è Windows (fondamentale per le tue licenze Pro/Server)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Recupera la versione specifica di Windows
                var osVersion = Environment.OSVersion;
                Console.WriteLine($"Stato OS: Windows rilevato correttamente.");
                Console.WriteLine($"Versione Kernel: {osVersion.VersionString}");
                
                // Nota tecnica: Qui in futuro potremo aggiungere il check per distinguere 11 Pro da Server 2025
            }
            else
            {
                // Messaggio di avviso se per caso lo script finisce su Linux o Mac
                Console.WriteLine("Attenzione: Sistema operativo non Windows rilevato.");
            }

            // Messaggio finale di conferma operatività
            Console.WriteLine("\n[OK] Tutti i moduli del Lab sono pronti per lo sviluppo.");
            Console.WriteLine("Premi un tasto per uscire...");
            Console.ReadKey(); // Tiene aperta la finestra finché non premi un tasto
        }
    }
}