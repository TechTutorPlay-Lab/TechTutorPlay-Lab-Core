using System;
using TechTutorPlay;

namespace TechTutorPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TechTutorPlay Enterprise Manager ===\n");

            // Crea il gestore studenti
            var manager = new StudentManager();

            // Crea alcuni studenti
            var daniele = new Student(1, "Daniele");
            var marco = new Student(2, "Marco Rossi");
            var lucia = new Student(3, "Lucia Bianchi");

            // Aggiungi studenti al manager
            manager.AggiungiStudente(daniele);
            manager.AggiungiStudente(marco);
            manager.AggiungiStudente(lucia);

            // Aggiungi voti
            daniele.AggiungiVoto(8.5);
            daniele.AggiungiVoto(9.0);
            daniele.AggiungiVoto(7.5);

            marco.AggiungiVoto(6.0);
            marco.AggiungiVoto(6.5);
            marco.AggiungiVoto(7.0);

            lucia.AggiungiVoto(9.5);
            lucia.AggiungiVoto(10.0);
            lucia.AggiungiVoto(9.0);

            Console.WriteLine("\n=== Riepilogo Studenti ===");
            manager.StampaRiepilogoClasse();

            Console.WriteLine("\n=== Dettagli Studente: Daniele ===");
            Console.WriteLine(daniele.GetDettagli());

            Console.WriteLine("\n---------------------------------------------");
            Console.WriteLine("Premi un tasto per chiudere il sistema...");
            Console.ReadKey();
        }
    }
}