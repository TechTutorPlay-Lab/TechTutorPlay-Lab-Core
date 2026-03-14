using System;
using Xunit;
using TechTutorPlay;

namespace TechTutorPlay.Tests.Fixtures
{
    /// <summary>
    /// Fixture per condividere setup tra test correlati.
    /// Viene creata una volta per ogni classe di test che la usa.
    /// </summary>
    public class StudentFixture : IDisposable
    {
        public Student EmptyStudent { get; }
        public Student StudentWithGrades { get; }
        public Student PassingStudent { get; }
        public Student FailingStudent { get; }

        public List<string> LogMessages { get; }

        public StudentFixture()
        {
            LogMessages = new List<string>();
            Action<string> logger = msg => LogMessages.Add(msg);

            EmptyStudent = new Student(1, "Empty Student", logger);
            
            StudentWithGrades = new Student(2, "Student With Grades", logger);
            StudentWithGrades.AggiungiVoto(6);
            StudentWithGrades.AggiungiVoto(7);
            StudentWithGrades.AggiungiVoto(8);

            PassingStudent = new Student(3, "Passing Student", logger);
            PassingStudent.AggiungiVoto(7);
            PassingStudent.AggiungiVoto(8);
            PassingStudent.AggiungiVoto(9);

            FailingStudent = new Student(4, "Failing Student", logger);
            FailingStudent.AggiungiVoto(4);
            FailingStudent.AggiungiVoto(5);
            FailingStudent.AggiungiVoto(5);
        }

        public void Dispose()
        {
            // Cleanup se necessario
            LogMessages.Clear();
        }
    }

    /// <summary>
    /// Collection fixture per condividere setup tra più classi di test.
    /// </summary>
    [CollectionDefinition("Student Collection")]
    public class StudentCollection : ICollectionFixture<StudentFixture>
    {
        // Questa classe non ha codice, serve solo per la definizione
    }
}