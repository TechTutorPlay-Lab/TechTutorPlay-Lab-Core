using System.Collections.Generic;
using TechTutorPlay;

namespace TechTutorPlay.Tests.Helpers
{
    /// <summary>
    /// Factory methods per creare oggetti Student nei test.
    /// Segue il pattern Object Mother.
    /// </summary>
    public static class StudentTestHelper
    {
        private static int _nextId = 1;

        public static Student CreateValidStudent(int? id = null, string? name = null)
        {
            return new Student(
                id ?? _nextId++,
                name ?? $"Test Student {_nextId}"
            );
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

        public static Student CreatePassingStudent()
        {
            return CreateStudentWithGrades(7, 8, 9);
        }

        public static Student CreateFailingStudent()
        {
            return CreateStudentWithGrades(4, 5, 5);
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

        /// <summary>
        /// Reset del counter ID per test isolati
        /// </summary>
        public static void ResetIdCounter()
        {
            _nextId = 1;
        }
    }
}