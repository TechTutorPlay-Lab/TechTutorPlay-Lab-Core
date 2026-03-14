using FluentAssertions;
using Xunit;
using TechTutorPlay;

namespace TechTutorPlay.Tests
{
    public class StudentTestsWithFluentAssertions
    {
        [Fact]
        public void AggiungiVoto_ConVotoValido_DeveAggiungerlo()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            student.AggiungiVoto(7.5);

            // Assert
            student.Voti.Should().ContainSingle()
                .Which.Should().Be(7.5);
        }

        [Fact]
        public void CalcolaMedia_ConTreVoti_DeveCalcolareMediaCorretta()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            media.Should().Be(7);
        }

        [Fact]
        public void Constructor_ConIdNegativo_DeveLanciareEccezione()
        {
            // Act
            Action act = () => new Student(-1, "Mario Rossi");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName("id")
                .WithMessage("L'ID deve essere maggiore di zero.*");
        }
    }
}