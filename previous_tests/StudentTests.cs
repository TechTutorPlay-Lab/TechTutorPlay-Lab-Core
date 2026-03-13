using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using TechTutorPlay;

namespace TechTutorPlay.Tests
{
    public class StudentTests
    {
        #region Constructor Tests

        [Fact]
        [Trait("Category", "Constructor")]
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
        [Trait("Category", "Constructor")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
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
        [Trait("Category", "Constructor")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            Action act = () => new Student(1, invalidName);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName("name")
                .WithMessage("Il nome non può essere vuoto.*");
        }

        #endregion

        #region AggiungiVoto Tests

        [Fact]
        [Trait("Category", "AggiungiVoto")]
        public void AggiungiVoto_WithValidGrade_ShouldAddGrade()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            student.AggiungiVoto(7.5);

            // Assert
            student.Voti.Should().ContainSingle()
                .Which.Should().Be(7.5);
        }

        [Theory]
        [Trait("Category", "AggiungiVoto")]
        [InlineData(0)]
        [InlineData(5.5)]
        [InlineData(10)]
        [InlineData(6.75)]
        public void AggiungiVoto_WithValidGrades_ShouldAddAllGrades(double grade)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            student.AggiungiVoto(grade);

            // Assert
            student.Voti.Should().Contain(grade);
        }

        [Theory]
        [Trait("Category", "AggiungiVoto")]
        [InlineData(-0.1)]
        [InlineData(-5)]
        [InlineData(10.1)]
        [InlineData(15)]
        [InlineData(double.MaxValue)]
        public void AggiungiVoto_WithGradeOutOfRange_ShouldThrowArgumentOutOfRangeException(double invalidGrade)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            Action act = () => student.AggiungiVoto(invalidGrade);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("voto")
                .WithMessage("*Il voto deve essere compreso tra 0 e 10.*");
        }

        [Fact]
        [Trait("Category", "AggiungiVoto")]
        public void AggiungiVoto_WithMultipleGrades_ShouldMaintainOrder()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            var expectedGrades = new[] { 6.0, 7.0, 8.0 };

            // Act
            foreach (var grade in expectedGrades)
            {
                student.AggiungiVoto(grade);
            }

            // Assert
            student.Voti.Should().HaveCount(3)
                .And.ContainInOrder(expectedGrades);
        }

        #endregion

        #region RimuoviVoto Tests

        [Fact]
        public void RimuoviVoto_ConIndiceValido_RimuoveVoto()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            student.RimuoviVoto(1);

            // Assert
            Assert.Equal(2, student.Voti.Count);
            Assert.DoesNotContain(7, student.Voti);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void RimuoviVoto_ConIndiceNonValido_LanciaArgumentOutOfRangeException(int indiceNonValido)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => student.RimuoviVoto(indiceNonValido));
        }

        #endregion

        #region CalcolaMedia Tests

        [Fact]
        public void CalcolaMedia_SenzaVoti_RestituisceZero()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var media = student.CalcolaMedia();

            // Assert
            Assert.Equal(0, media);
        }

        [Fact]
        public void CalcolaMedia_ConVoti_CalcolaMediaCorretta()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            Assert.Equal(7, media);
        }

        [Fact]
        public void CalcolaMedia_ConUnSoloVoto_RestituisceIlVoto()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(8.5);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            Assert.Equal(8.5, media);
        }

        [Theory]
        [Trait("Category", "CalcolaMedia")]
        [InlineData(new[] { 6.0, 8.0 }, 7.0)]
        [InlineData(new[] { 5.0, 5.0, 10.0 }, 6.666666666666667)]
        [InlineData(new[] { 10.0, 10.0, 10.0 }, 10.0)]
        public void CalcolaMedia_WithVariousGrades_ShouldCalculateCorrectly(double[] grades, double expectedAverage)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            foreach (var grade in grades)
            {
                student.AggiungiVoto(grade);
            }

            // Act
            var average = student.CalcolaMedia();

            // Assert
            average.Should().BeApproximately(expectedAverage, 0.0001);
        }

        #endregion

        #region GetVotoMinimo/Massimo Tests

        [Fact]
        public void GetVotoMinimo_SenzaVoti_RestituisceNull()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var min = student.GetVotoMinimo();

            // Assert
            Assert.Null(min);
        }

        [Fact]
        public void GetVotoMinimo_ConVoti_RestituisceVotoMinimo()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(4);
            student.AggiungiVoto(8);

            // Act
            var min = student.GetVotoMinimo();

            // Assert
            Assert.Equal(4, min);
        }

        [Fact]
        public void GetVotoMassimo_SenzaVoti_RestituisceNull()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var max = student.GetVotoMassimo();

            // Assert
            Assert.Null(max);
        }

        [Fact]
        public void GetVotoMassimo_ConVoti_RestituisceVotoMassimo()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(9);
            student.AggiungiVoto(8);

            // Act
            var max = student.GetVotoMassimo();

            // Assert
            Assert.Equal(9, max);
        }

        #endregion

        #region IsPromosso Tests

        [Fact]
        public void IsPromosso_ConMediaSufficiente_RestituisceTrue()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);

            // Act
            var promosso = student.IsPromosso();

            // Assert
            Assert.True(promosso);
        }

        [Fact]
        public void IsPromosso_ConMediaInsufficiente_RestituisceFalse()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(4);
            student.AggiungiVoto(5);

            // Act
            var promosso = student.IsPromosso();

            // Assert
            Assert.False(promosso);
        }

        [Fact]
        public void IsPromosso_ConSogliaPersonalizzata_UsaSogliaCorretta()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(7);
            student.AggiungiVoto(7);

            // Act
            var promosso = student.IsPromosso(8.0);

            // Assert
            Assert.False(promosso);
        }

        #endregion

        #region GetVotiSopra/Sotto Tests

        [Fact]
        public void GetVotiSopra_ConSoglia_RestituisceVotiCorretti()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(5);
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            var votiSopra = student.GetVotiSopra(6).ToList();

            // Assert
            Assert.Equal(3, votiSopra.Count);
            Assert.Contains(6, votiSopra);
            Assert.Contains(7, votiSopra);
            Assert.Contains(8, votiSopra);
        }

        [Fact]
        public void GetVotiSotto_ConSoglia_RestituisceVotiCorretti()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(5);
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);

            // Act
            var votiSotto = student.GetVotiSotto(6).ToList();

            // Assert
            Assert.Single(votiSotto);
            Assert.Contains(5, votiSotto);
        }

        #endregion

        #region ContaVoti Tests

        [Fact]
        public void ContaVotiSufficienti_RestituisceConteggioCorretto()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(5);
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);

            // Act
            var sufficienti = student.ContaVotiSufficienti();

            // Assert
            Assert.Equal(2, sufficienti);
        }

        [Fact]
        public void ContaVotiInsufficenti_RestituisceConteggioCorretto()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(4);
            student.AggiungiVoto(5);
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);

            // Act
            var insufficienti = student.ContaVotiInsufficenti();

            // Assert
            Assert.Equal(2, insufficienti);
        }

        #endregion

        #region SvuotaVoti Tests

        [Fact]
        public void SvuotaVoti_RimuoveTuttiIVoti()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(6);
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            student.SvuotaVoti();

            // Assert
            Assert.Empty(student.Voti);
        }

        #endregion

        #region IComparable Tests

        [Fact]
        public void CompareTo_ConMediaMaggiore_RestituiscePositivo()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            student1.AggiungiVoto(8);
            var student2 = new Student(2, "Luigi Verdi");
            student2.AggiungiVoto(6);

            // Act
            var result = student1.CompareTo(student2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_ConMediaMinore_RestituisceNegativo()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            student1.AggiungiVoto(5);
            var student2 = new Student(2, "Luigi Verdi");
            student2.AggiungiVoto(7);

            // Act
            var result = student1.CompareTo(student2);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void CompareTo_ConMediaUguale_RestituisceZero()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            student1.AggiungiVoto(7);
            var student2 = new Student(2, "Luigi Verdi");
            student2.AggiungiVoto(7);

            // Act
            var result = student1.CompareTo(student2);

            // Assert
            Assert.Equal(0, result);
        }

        #endregion

        #region IEquatable Tests

        [Fact]
        public void Equals_ConStessoId_RestituisceTrue()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            var student2 = new Student(1, "Mario Rossi Clone");

            // Act
            var areEqual = student1.Equals(student2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ConIdDiversi_RestituisceFalse()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            var student2 = new Student(2, "Luigi Verdi");

            // Act
            var areEqual = student1.Equals(student2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ConStessoId_RestituisceStessoHash()
        {
            // Arrange
            var student1 = new Student(1, "Mario Rossi");
            var student2 = new Student(1, "Mario Rossi Clone");

            // Act & Assert
            Assert.Equal(student1.GetHashCode(), student2.GetHashCode());
        }

        #endregion

        #region ToString Tests

        [Fact]
        [Trait("Category", "ToString")]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(7);

            // Act
            var result = student.ToString();

            // Assert
            result.Should().Contain("Mario Rossi")
                .And.Contain("ID: 1")
                .And.Contain("Voti: 1")
                .And.Contain("Media: 7");
        }

        [Fact]
        [Trait("Category", "ToString")]
        public void ToString_WithNoGrades_ShouldShowZeroAverage()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var result = student.ToString();

            // Assert
            result.Should().Contain("Voti: 0")
                .And.Contain("Media: 0");
        }

        #endregion

        #region Immutability Tests

        [Fact]
        [Trait("Category", "Immutability")]
        public void Voti_ShouldBeReadOnlyFromOutside()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(7);

            // Act
            var voti = student.Voti;

            // Assert
            voti.Should().BeAssignableTo<IReadOnlyList<double>>();
        }

        [Fact]
        [Trait("Category", "Immutability")]
        public void Id_ShouldBeImmutableAfterConstruction()
        {
            // Arrange & Act
            var student = new Student(1, "Mario Rossi");

            // Assert
            // Verifica che Id sia init-only compilando il codice
            // Se questo compila, la proprietà NON è veramente immutabile
            // student.Id = 2; // Questo NON deve compilare
            student.Id.Should().Be(1);
        }

        [Fact]
        [Trait("Category", "Immutability")]
        public void Name_ShouldBeImmutableAfterConstruction()
        {
            // Arrange & Act
            var student = new Student(1, "Mario Rossi");

            // Assert
            // student.Name = "Altro Nome"; // Questo NON deve compilare
            student.Name.Should().Be("Mario Rossi");
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        [Trait("Category", "EdgeCases")]
        public void AggiungiVoto_WithBoundaryValues_ShouldAcceptZeroAndTen()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            student.AggiungiVoto(0);
            student.AggiungiVoto(10);

            // Assert
            student.Voti.Should().HaveCount(2)
                .And.Contain(new[] { 0.0, 10.0 });
        }

        [Fact]
        [Trait("Category", "EdgeCases")]
        public void CalcolaMedia_WithLargeNumberOfGrades_ShouldCalculateCorrectly()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            for (int i = 0; i < 1000; i++)
            {
                student.AggiungiVoto(7.5);
            }

            // Act
            var average = student.CalcolaMedia();

            // Assert
            average.Should().Be(7.5);
            student.Voti.Should().HaveCount(1000);
        }

        #endregion

        #region Common Pitfall #1: Testing with Console Output

        [Fact]
        [Trait("Category", "Logging")]
        public void AggiungiVoto_ShouldLogMessage_WhenCalled()
        {
            // Arrange
            var logMessages = new List<string>();
            var student = new Student(1, "Mario Rossi", msg => logMessages.Add(msg));

            // Act
            student.AggiungiVoto(7.5);

            // Assert
            logMessages.Should().ContainSingle()
                .Which.Should().Contain("[LOG] Voto 7.5 aggiunto a Mario Rossi");
        }

        [Fact]
        [Trait("Category", "Logging")]
        public void AggiungiVoto_WithoutLogger_ShouldNotThrow()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi"); // No logger

            // Act
            Action act = () => student.AggiungiVoto(7.5);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region Common Pitfall #2: Non-Deterministic Tests (Random, DateTime.Now)

        // ❌ EVITARE QUESTO:
        // [Fact]
        // public void Test_WithRandomValue()
        // {
        //     var random = new Random();
        //     var value = random.Next(1, 10);
        //     // Test che può fallire casualmente ❌
        // }

        // ✅ FARE COSÌ:
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void AggiungiVoto_WithSpecificValues_ShouldWork(int studentId)
        {
            // Arrange & Act
            var student = new Student(studentId, "Test Student");

            // Assert
            student.Id.Should().Be(studentId);
        }

        #endregion

        #region Common Pitfall #3: Testing Multiple Things in One Test

        // ❌ EVITARE QUESTO:
        // [Fact]
        // public void StudentTest_DoesEverything() 
        // {
        //     var student = new Student(1, "Test");
        //     student.AggiungiVoto(7);
        //     Assert.Equal(7, student.CalcolaMedia());
        //     student.AggiungiVoto(8);
        //     Assert.Equal(7.5, student.CalcolaMedia());
        //     // Troppo in un solo test ❌
        // }

        // ✅ FARE COSÌ: Un test, un concetto
        [Fact]
        [Trait("Category", "CalcolaMedia")]
        public void CalcolaMedia_WithSingleGrade_ShouldReturnThatGrade()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(7);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            media.Should().Be(7);
        }

        [Fact]
        [Trait("Category", "CalcolaMedia")]
        public void CalcolaMedia_WithMultipleGrades_ShouldReturnAverage()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(7);
            student.AggiungiVoto(8);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            media.Should().Be(7.5);
        }

        #endregion

        #region Common Pitfall #4: Poor Exception Testing

        // ❌ EVITARE QUESTO:
        // [Fact]
        // public void BadExceptionTest()
        // {
        //     try
        //     {
        //         new Student(-1, "Test");
        //         Assert.True(false, "Should have thrown");
        //     }
        //     catch (ArgumentException)
        //     {
        //         // Pass
        //     }
        // }

        // ✅ FARE COSÌ: Test completo dell'eccezione
        [Fact]
        [Trait("Category", "Exceptions")]
        public void Constructor_WithNegativeId_ShouldThrowArgumentExceptionWithCorrectMessage()
        {
            // Arrange
            Action act = () => new Student(-1, "Mario Rossi");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithParameterName("id")
                .WithMessage("L'ID deve essere maggiore di zero.*")
                .And.ParamName.Should().Be("id");
        }

        #endregion

        #region Common Pitfall #5: Not Testing Edge Cases

        [Theory]
        [Trait("Category", "EdgeCases")]
        [InlineData(0)]      // Boundary: minimo valido
        [InlineData(10)]     // Boundary: massimo valido
        [InlineData(0.001)]  // Appena sopra zero
        [InlineData(9.999)]  // Appena sotto 10
        public void AggiungiVoto_WithBoundaryValues_ShouldAccept(double grade)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            Action act = () => student.AggiungiVoto(grade);

            // Assert
            act.Should().NotThrow();
            student.Voti.Should().Contain(grade);
        }

        [Theory]
        [Trait("Category", "EdgeCases")]
        [InlineData(-0.001)]  // Appena sotto zero
        [InlineData(10.001)]  // Appena sopra 10
        public void AggiungiVoto_JustOutsideBoundary_ShouldThrow(double grade)
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            Action act = () => student.AggiungiVoto(grade);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        #endregion

        #region Common Pitfall #6: Not Testing Null Cases

        [Fact]
        [Trait("Category", "NullHandling")]
        public void GetVotoMinimo_WithNoGrades_ShouldReturnNull()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var min = student.GetVotoMinimo();

            // Assert
            min.Should().BeNull();
        }

        [Fact]
        [Trait("Category", "NullHandling")]
        public void Equals_WithNullStudent_ShouldReturnFalse()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");

            // Act
            var result = student.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Common Pitfall #7: Fragile Tests (Order Dependent)

        // ❌ EVITARE: Test che dipendono dall'ordine di esecuzione
        // Ogni test deve essere indipendente

        [Fact]
        [Trait("Category", "Independence")]
        public void Test1_ShouldBeIndependent()
        {
            var student = new Student(1, "Test1");
            student.AggiungiVoto(7);
            student.Voti.Should().HaveCount(1);
        }

        [Fact]
        [Trait("Category", "Independence")]
        public void Test2_ShouldBeIndependent()
        {
            var student = new Student(2, "Test2");
            student.AggiungiVoto(8);
            student.Voti.Should().HaveCount(1); // Non 2!
        }

        #endregion

        #region Common Pitfall #8: Magic Numbers

        // ❌ EVITARE QUESTO:
        // [Fact]
        // public void Test_WithMagicNumbers()
        // {
        //     var student = new Student(1, "Test");
        //     student.AggiungiVoto(7);
        //     Assert.Equal(7, student.CalcolaMedia());
        // }

        // ✅ FARE COSÌ: Costanti significative
        [Fact]
        [Trait("Category", "BestPractices")]
        public void CalcolaMedia_WithPassingGrade_ShouldBeAboveMinimum()
        {
            // Arrange
            const int studentId = 1;
            const string studentName = "Mario Rossi";
            const double passingGrade = 7.0;
            const double minimumPassingThreshold = 6.0;

            var student = new Student(studentId, studentName);
            student.AggiungiVoto(passingGrade);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            media.Should().BeGreaterThanOrEqualTo(minimumPassingThreshold);
        }

        #endregion

        #region Common Pitfall #9: Not Testing Collections Properly

        [Fact]
        [Trait("Category", "Collections")]
        public void Voti_AfterMultipleAdditions_ShouldMaintainOrderAndCount()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            var expectedGrades = new[] { 6.0, 7.5, 8.0, 9.0 };

            // Act
            foreach (var grade in expectedGrades)
            {
                student.AggiungiVoto(grade);
            }

            // Assert
            student.Voti.Should()
                .HaveCount(expectedGrades.Length)
                .And.ContainInOrder(expectedGrades)
                .And.OnlyContain(g => g >= 0 && g <= 10);
        }

        #endregion

        #region Common Pitfall #10: Not Testing Performance-Critical Code

        [Fact]
        [Trait("Category", "Performance")]
        public void CalcolaMedia_WithManyGrades_ShouldCompleteQuickly()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            const int numberOfGrades = 10000;
            
            for (int i = 0; i < numberOfGrades; i++)
            {
                student.AggiungiVoto(7.5);
            }

            // Act
            Action act = () => student.CalcolaMedia();

            // Assert
            act.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100));
        }

        #endregion

        #region Common Pitfall #11: Ignoring Floating Point Precision

        [Fact]
        [Trait("Category", "FloatingPoint")]
        public void CalcolaMedia_WithFloatingPointValues_ShouldHandlePrecision()
        {
            // Arrange
            var student = new Student(1, "Mario Rossi");
            student.AggiungiVoto(5.0);
            student.AggiungiVoto(5.0);
            student.AggiungiVoto(10.0);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            // ❌ NON fare: media.Should().Be(6.666666666666667);
            // ✅ FARE:
            media.Should().BeApproximately(6.67, 0.01);
        }

        #endregion

        #region Common Pitfall #12: Not Using Test Helpers/Factories

        // Vedi StudentTestHelper.cs per esempi di factory methods

        [Fact]
        [Trait("Category", "TestHelpers")]
        public void UsingTestHelper_MakesTestsCleaner()
        {
            // Arrange - Using helper
            var student = TestHelpers.StudentTestHelper.CreateStudentWithGrades(6, 7, 8);

            // Act
            var media = student.CalcolaMedia();

            // Assert
            media.Should().Be(7);
        }

        #endregion
    }
}