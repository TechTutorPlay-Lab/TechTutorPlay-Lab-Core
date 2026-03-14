using TechTutorPlay;

namespace TechTutorPlay.Tests.TestHelpers
{
    /// <summary>
    /// Helper class per creare oggetti Student di test con configurazioni comuni.
    /// </summary>
    public static class StudentTestHelper
    {
        public static Student CreateValidStudent(int id = 1, string name = "Test Student")
        {
            return new Student(id, name);
        }

        public static Student CreateStudentWithGrades(params double[] grades)
        {
            var student = CreateValidStudent();
            foreach (var grade in grades)
            {
                student.AggiungiVoto(grade);
            }
            return student;
        }

        public static Student CreateStudentWithAverageGrade(double targetAverage, int numberOfGrades = 3)
        {
            var student = CreateValidStudent();
            for (int i = 0; i < numberOfGrades; i++)
            {
                student.AggiungiVoto(targetAverage);
            }
            return student;
        }
    }
}