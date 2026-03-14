using Xunit;
using FluentAssertions;
using TechTutorPlay.Tests.Fixtures;

namespace TechTutorPlay.Tests.Unit
{
    [Collection("Student Collection")]
    [Trait("Category", "Unit")]
    public class StudentCalculationTestsWithFixture
    {
        private readonly StudentFixture _fixture;

        public StudentCalculationTestsWithFixture(StudentFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void EmptyStudent_ShouldHaveZeroAverage()
        {
            // Act
            var average = _fixture.EmptyStudent.CalcolaMedia();

            // Assert
            average.Should().Be(0);
        }

        [Fact]
        public void StudentWithGrades_ShouldHaveCorrectAverage()
        {
            // Act
            var average = _fixture.StudentWithGrades.CalcolaMedia();

            // Assert
            average.Should().Be(7);
        }

        [Fact]
        public void PassingStudent_ShouldBePassed()
        {
            // Act
            var isPassed = _fixture.PassingStudent.IsPromosso();

            // Assert
            isPassed.Should().BeTrue();
        }
    }
}