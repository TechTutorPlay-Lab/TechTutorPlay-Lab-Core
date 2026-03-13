using Bogus;
using TechTutorPlay;

namespace TechTutorPlay.Tests.Helpers
{
    /// <summary>
    /// Genera dati di test realistici usando Bogus.
    /// </summary>
    public static class TestDataGenerator
    {
        private static readonly Faker<Student> StudentFaker = new Faker<Student>()
            .CustomInstantiator(f => new Student(
                f.IndexFaker + 1,
                f.Name.FullName()
            ));

        public static Student GenerateStudent()
        {
            return StudentFaker.Generate();
        }

        public static List<Student> GenerateStudents(int count)
        {
            return StudentFaker.Generate(count);
        }

        public static Student GenerateStudentWithGrades(int gradeCount = 5)
        {
            var student = GenerateStudent();
            var faker = new Faker();
            
            for (int i = 0; i < gradeCount; i++)
            {
                student.AggiungiVoto(faker.Random.Double(0, 10));
            }
            
            return student;
        }
    }
}