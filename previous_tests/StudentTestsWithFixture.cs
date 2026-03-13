using Xunit;
using FluentAssertions;
using TechTutorPlay;

namespace TechTutorPlay.Tests
{
    /// <summary>
    /// Fixture per condividere setup tra test correlati.
    /// </summary>
    public class StudentFixture : IDisposable
    {
        public Student DefaultStudent { get; }
        public Student StudentWithGrades { get; }

        public StudentFixture()
        {
            DefaultStudent = new Student(1, "Mario Rossi");
            
            StudentWithGrades = new Student(2, "Luigi Verdi");
            StudentWithGrades.AggiungiVoto(6);
            StudentWithGrades.AggiungiVoto(7);
            StudentWithGrades.AggiungiVoto(8);
        }

        public void Dispose()
        {
            // Cleanup se necessario
        }
    }

    public class StudentTestsWithFixture : IClassFixture<StudentFixture>
    {
        private readonly StudentFixture _fixture;

        public StudentTestsWithFixture(StudentFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void DefaultStudent_ShouldHaveNoGrades()
        {
            _fixture.DefaultStudent.Voti.Should().BeEmpty();
        }

        [Fact]
        public void StudentWithGrades_ShouldHaveCorrectAverage()
        {
            _fixture.StudentWithGrades.CalcolaMedia().Should().Be(7);
        }
    }
}