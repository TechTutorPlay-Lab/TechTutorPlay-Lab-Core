using TechTutorPlay;

namespace TechTutorPlay.Tests.Helpers
{
    /// <summary>
    /// Builder pattern per creare oggetti Student complessi nei test.
    /// Più flessibile di Object Mother per scenari complessi.
    /// </summary>
    public class StudentTestDataBuilder
    {
        private int _id = 1;
        private string _name = "Default Student";
        private List<double> _grades = new();
        private Action<string>? _logger;

        public StudentTestDataBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public StudentTestDataBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public StudentTestDataBuilder WithGrade(double grade)
        {
            _grades.Add(grade);
            return this;
        }

        public StudentTestDataBuilder WithGrades(params double[] grades)
        {
            _grades.AddRange(grades);
            return this;
        }

        public StudentTestDataBuilder WithPassingGrades()
        {
            _grades = new List<double> { 7, 8, 9 };
            return this;
        }

        public StudentTestDataBuilder WithFailingGrades()
        {
            _grades = new List<double> { 4, 5, 5 };
            return this;
        }

        public StudentTestDataBuilder WithLogger(Action<string> logger)
        {
            _logger = logger;
            return this;
        }

        public Student Build()
        {
            var student = new Student(_id, _name, _logger);
            foreach (var grade in _grades)
            {
                student.AggiungiVoto(grade);
            }
            return student;
        }

        /// <summary>
        /// Crea un builder pre-configurato per uno studente valido
        /// </summary>
        public static StudentTestDataBuilder AValidStudent()
        {
            return new StudentTestDataBuilder();
        }
    }
}