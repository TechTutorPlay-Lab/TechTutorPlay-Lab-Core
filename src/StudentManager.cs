using System;
using System.Collections.Generic;
using System.Linq;

namespace TechTutorPlay
{
    /// <summary>
    /// Gestisce le operazioni sulla collezione di studenti.
    /// </summary>
    public class StudentManager
    {
        private readonly List<Student> _students = [];
        private readonly Action<string>? _logger;

        public IReadOnlyList<Student> Students => _students.AsReadOnly();

        public StudentManager(Action<string>? logger = null)
        {
            _logger = logger ?? (msg => Console.WriteLine(msg));
        }

        public void AggiungiStudente(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (_students.Any(s => s.Id == student.Id))
                throw new InvalidOperationException($"Uno studente con ID {student.Id} esiste già.");

            _students.Add(student);
            _logger?.Invoke($"[LOG] Studente {student.Name} (ID: {student.Id}) aggiunto alla classe.");
        }

        public void RimuoviStudente(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                throw new InvalidOperationException($"Studente con ID {id} non trovato.");

            _students.Remove(student);
            _logger?.Invoke($"[LOG] Studente {student.Name} (ID: {student.Id}) rimosso dalla classe.");
        }

        public Student? TrovaStudente(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public Student? TrovaStudentePerNome(string name)
        {
            return _students.FirstOrDefault(s => 
                s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Student> StudentiPromossi(double sogliaMinima = 6.0)
        {
            return _students.Where(s => s.IsPromosso(sogliaMinima));
        }

        public IEnumerable<Student> StudentiNonPromossi(double sogliaMinima = 6.0)
        {
            return _students.Where(s => !s.IsPromosso(sogliaMinima));
        }

        public double CalcolaMediaClasse()
        {
            if (_students.Count == 0)
                return 0;

            return _students.Average(s => s.CalcolaMedia());
        }

        public void StampaRiepilogoClasse()
        {
            Console.WriteLine("=== TechTutorPlay: Gestione Classe Virtuale ===");
            Console.WriteLine($"Studenti totali iscritti: {_students.Count}");
            Console.WriteLine("---------------------------------------------");

            foreach (var student in _students.OrderBy(s => s.Id))
            {
                var status = student.Voti.Count > 0 
                    ? (student.IsPromosso() ? "Promosso" : "Non Promosso")
                    : "Nessun voto";

                Console.WriteLine($"ID: {student.Id} | Studente: {student.Name} | Media: {student.CalcolaMedia():F2} | Status: {status}");
            }

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"Media classe: {CalcolaMediaClasse():F2}");
            Console.WriteLine("Sincronizzazione con il database completata.");
        }
    }
}
