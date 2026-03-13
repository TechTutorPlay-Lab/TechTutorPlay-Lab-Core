using System;
using System.Collections.Generic;
using System.Linq;

namespace TechTutorPlay
{
    public class Student : IComparable<Student>, IEquatable<Student>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public IReadOnlyList<double> Voti => _voti.AsReadOnly();

        private readonly List<double> _voti = [];
        private readonly Action<string>? _logger;

        // Costruttore con logger opzionale (testabile)
        public Student(int id, string name, Action<string>? logger = null)
        {
            if (id <= 0)
                throw new ArgumentException("L'ID deve essere maggiore di zero.", nameof(id));
            
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Il nome non può essere vuoto.", nameof(name));

            Id = id;
            Name = name;
            _logger = logger ?? (msg => Console.WriteLine(msg)); // Default to Console
        }

        public void AggiungiVoto(double voto)
        {
            if (voto < 0 || voto > 10)
                throw new ArgumentOutOfRangeException(nameof(voto), "Il voto deve essere compreso tra 0 e 10.");

            _voti.Add(voto);
            _logger?.Invoke($"[LOG] Voto {voto} aggiunto a {Name}.");
        }

        public void RimuoviVoto(int indice)
        {
            if (indice < 0 || indice >= _voti.Count)
                throw new ArgumentOutOfRangeException(nameof(indice), "Indice non valido.");

            double votoRimosso = _voti[indice];
            _voti.RemoveAt(indice);
            _logger?.Invoke($"[LOG] Voto {votoRimosso} rimosso da {Name}.");
        }

        public double CalcolaMedia()
        {
            return _voti.Count > 0 ? _voti.Average() : 0;
        }

        public double? GetVotoMinimo()
        {
            return _voti.Count > 0 ? _voti.Min() : null;
        }

        public double? GetVotoMassimo()
        {
            return _voti.Count > 0 ? _voti.Max() : null;
        }

        public bool IsPromosso(double sogliaMinima = 6.0)
        {
            return CalcolaMedia() >= sogliaMinima;
        }

        public IEnumerable<double> GetVotiSopra(double soglia)
        {
            return _voti.Where(v => v >= soglia);
        }

        public IEnumerable<double> GetVotiSotto(double soglia)
        {
            return _voti.Where(v => v < soglia);
        }

        public int ContaVotiSufficienti(double sufficienza = 6.0)
        {
            return _voti.Count(v => v >= sufficienza);
        }

        public int ContaVotiInsufficenti(double sufficienza = 6.0)
        {
            return _voti.Count(v => v < sufficienza);
        }

        public void SvuotaVoti()
        {
            _voti.Clear();
            _logger?.Invoke($"[LOG] Tutti i voti di {Name} sono stati rimossi.");
        }

        public int CompareTo(Student? other)
        {
            if (other is null) return 1;
            return CalcolaMedia().CompareTo(other.CalcolaMedia());
        }

        public bool Equals(Student? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Student);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Studente: {Name} (ID: {Id}), Voti: {_voti.Count}, Media: {CalcolaMedia():F2}";
        }

        public string GetDettagli()
        {
            if (_voti.Count == 0)
                return $"{Name} (ID: {Id}) - Nessun voto registrato";

            return $"{Name} (ID: {Id})\n" +
                   $"  Voti: [{string.Join(", ", _voti)}]\n" +
                   $"  Media: {CalcolaMedia():F2}\n" +
                   $"  Min: {GetVotoMinimo():F2} | Max: {GetVotoMassimo():F2}\n" +
                   $"  Sufficienti: {ContaVotiSufficienti()} | Insufficienti: {ContaVotiInsufficenti()}\n" +
                   $"  Stato: {(IsPromosso() ? "Promosso" : "Non promosso")}";
        }
    }
}