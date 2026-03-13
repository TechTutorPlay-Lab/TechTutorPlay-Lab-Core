using System;
using Xunit;
using FluentAssertions;
using TechTutorPlay;
using TechTutorPlay.Tests.Helpers;

namespace TechTutorPlay.Tests.Unit
{
    /// <summary>
    /// Test unitari per la classe Student.
    /// Organizzati per funzionalità con regions e traits.
    /// </summary>
    [Trait("Category", "Unit")]
    [Trait("Component", "Student")]
    public class StudentTests
    {
        #region Constructor Tests

        public class ConstructorTests
        {
            [Fact]
            [Trait("Priority", "High")]
            public void Constructor_WithValidParameters_ShouldCreateStudent()
            {
                // Arrange & Act
                var student = new Student(1, "Mario Rossi");

                // Assert
                student.Id.Should().Be(1);
                student.Name.Should().Be("Mario Rossi");
                student.Voti.Should().BeEmpty();
            }

            [Theory]
            [Trait("Priority", "High")]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(int.MinValue)]
            public void Constructor_WithInvalidId_ShouldThrowArgumentException(int invalidId)
            {
                // Arrange
                Action act = () => new Student(invalidId, "Mario Rossi");

                // Assert
                act.Should().Throw<ArgumentException>()
                    .WithParameterName("id")
                    .WithMessage("L'ID deve essere maggiore di zero.*");
            }

            [Theory]
            [Trait("Priority", "High")]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public void Constructor_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
            {
                // Arrange
                Action act = () => new Student(1, invalidName!);

                // Assert
                act.Should().Throw<ArgumentException>()
                    .WithParameterName("name");
            }
        }

        #endregion

        #region Grade Management Tests

        public class GradeManagementTests
        {
            [Fact]
            [Trait("Priority", "High")]
            public void AggiungiVoto_WithValidGrade_ShouldAddGrade()
            {
                // Arrange
                var student = StudentTestHelper.CreateValidStudent();

                // Act
                student.AggiungiVoto(7.5);

                // Assert
                student.Voti.Should().ContainSingle()
                    .Which.Should().Be(7.5);
            }

            [Theory]
            [Trait("Priority", "Medium")]
            [InlineData(0)]
            [InlineData(5.5)]
            [InlineData(10)]
            public void AggiungiVoto_WithBoundaryValues_ShouldAccept(double grade)
            {
                // Arrange
                var student = StudentTestHelper.CreateValidStudent();

                // Act
                student.AggiungiVoto(grade);

                // Assert
                student.Voti.Should().Contain(grade);
            }

            [Theory]
            [Trait("Priority", "High")]
            [InlineData(-0.1)]
            [InlineData(10.1)]
            public void AggiungiVoto_WithInvalidGrade_ShouldThrowException(double invalidGrade)
            {
                // Arrange
                var student = StudentTestHelper.CreateValidStudent();
                Action act = () => student.AggiungiVoto(invalidGrade);

                // Assert
                act.Should().Throw<ArgumentOutOfRangeException>()
                    .WithParameterName("voto");
            }
        }

        #endregion

        #region Calculation Tests

        public class CalculationTests
        {
            [Fact]
            [Trait("Priority", "High")]
            public void CalcolaMedia_WithNoGrades_ShouldReturnZero()
            {
                // Arrange
                var student = StudentTestHelper.CreateValidStudent();

                // Act
                var media = student.CalcolaMedia();

                // Assert
                media.Should().Be(0);
            }

            [Theory]
            [Trait("Priority", "High")]
            [InlineData(new[] { 6.0, 7.0, 8.0 }, 7.0)]
            [InlineData(new[] { 10.0, 10.0, 10.0 }, 10.0)]
            [InlineData(new[] { 5.0, 5.0, 10.0 }, 6.666666666666667)]
            public void CalcolaMedia_WithGrades_ShouldCalculateCorrectly(double[] grades, double expected)
            {
                // Arrange
                var student = StudentTestHelper.CreateStudentWithGrades(grades);

                // Act
                var media = student.CalcolaMedia();

                // Assert
                media.Should().BeApproximately(expected, 0.0001);
            }

            [Fact]
            [Trait("Priority", "Medium")]
            public void GetVotoMinimo_WithGrades_ShouldReturnMinimum()
            {
                // Arrange
                var student = StudentTestHelper.CreateStudentWithGrades(6, 4, 8);

                // Act
                var min = student.GetVotoMinimo();

                // Assert
                min.Should().Be(4);
            }

            [Fact]
            [Trait("Priority", "Medium")]
            public void GetVotoMassimo_WithGrades_ShouldReturnMaximum()
            {
                // Arrange
                var student = StudentTestHelper.CreateStudentWithGrades(6, 9, 8);

                // Act
                var max = student.GetVotoMassimo();

                // Assert
                max.Should().Be(9);
            }
        }

        #endregion

        #region Business Logic Tests

        public class BusinessLogicTests
        {
            [Theory]
            [Trait("Priority", "High")]
            [InlineData(new[] { 6.0, 7.0 }, true)]
            [InlineData(new[] { 4.0, 5.0 }, false)]
            [InlineData(new[] { 6.0 }, true)]
            public void IsPromosso_ShouldDeterminePassingStatus(double[] grades, bool expectedResult)
            {
                // Arrange
                var student = StudentTestHelper.CreateStudentWithGrades(grades);

                // Act
                var isPromosso = student.IsPromosso();

                // Assert
                isPromosso.Should().Be(expectedResult);
            }

            [Fact]
            [Trait("Priority", "Medium")]
            public void ContaVotiSufficienti_ShouldCountCorrectly()
            {
                // Arrange
                var student = StudentTestHelper.CreateStudentWithGrades(5, 6, 7, 4);

                // Act
                var count = student.ContaVotiSufficienti();

                // Assert
                count.Should().Be(2);
            }
        }

        #endregion
    }
}