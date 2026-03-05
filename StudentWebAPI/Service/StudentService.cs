using StudentWebAPI.Data;
using StudentWebAPI.Model;

namespace StudentWebAPI.Service
{
    public class StudentService
    {
        private readonly InMemoryStudentStore _store;

        public StudentService(InMemoryStudentStore store)
        {
            _store = store;
        }

        public List<Student> GetAll()
        {
            return _store.Students;
        }

        public Student Add(StudentDto dto)
        {
            var student = new Student
            {
                Id = _store.Students.Count + 1,
                Name = dto.Name,
                Age = dto.Age,
                Email = dto.Email
            };

            _store.Students.Add(student);
            return student;
        }

        public bool Delete(int id)
        {
            var student = _store.Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return false;

            _store.Students.Remove(student);
            return true;
        }
    }
}
