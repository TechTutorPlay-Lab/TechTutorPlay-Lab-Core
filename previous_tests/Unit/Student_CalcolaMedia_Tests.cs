using Xunit;
using FluentAssertions;
using TechTutorPlay;
using TechTutorPlay.Tests.Helpers;

namespace TechTutorPlay.Tests.Unit
{
    /// <summary>
    /// Alternative naming: Una classe per metodo testato.
    /// Utile quando un metodo ha molti scenari da testare.
    /// </summary>
    [Trait("Category", "Unit")]
    [Trait("Component", "Student")]
    [Trait("Method", "CalcolaMedia")]
    public class Student_CalcolaMedia_Tests
    {
        [Fact]
        public void Should_Return_Zero_When_No_Grades()
        {
            // Arrange
            var student = StudentTestHelper.CreateValidStudent();

            // Act
            var result = student.CalcolaMedia();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Should_Return_Correct_Average_When_Single_Grade()
        {
            // Arrange
            var student = StudentTestHelper.CreateStudentWithGrades(7.5);

            // Act
            var result = student.CalcolaMedia();

            // Assert
            result.Should().Be(7.5);
        }

        [Theory]
        [InlineData(new[] { 6.0, 7.0, 8.0 }, 7.0)]
        [InlineData(new[] { 10.0, 10.0, 10.0 }, 10.0)]
        public void Should_Calculate_Average_Correctly_When_Multiple_Grades(
            double[] grades, 
            double expectedAverage)
        {
            // Arrange
            var student = StudentTestHelper.CreateStudentWithGrades(grades);

            // Act
            var result = student.CalcolaMedia();

            // Assert
            result.Should().BeApproximately(expectedAverage, 0.0001);
        }
    }
}