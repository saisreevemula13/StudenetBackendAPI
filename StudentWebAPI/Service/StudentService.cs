using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;

namespace StudentWebAPI.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repo)
        {
            _repository = repo;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudents()
        {
            var students = await _repository.GetAllStudents();

            return students.Select(stud => new StudentDto
            {
                Id = stud.Id,
                Name = stud.Name,
                Email = stud.Email,
                Age = stud.Age,
                PhoneNumber = stud.PhoneNumber,          // added
                CreatedDate = stud.CreatedDate           //FIXED
            });
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            var stud = await _repository.GetStudentById(id);

            if (stud == null)
                return null;

            return new StudentDto
            {
                Id = stud.Id,
                Name = stud.Name,
                Email = stud.Email,
                Age = stud.Age,
                PhoneNumber = stud.PhoneNumber,          // added
                CreatedDate = stud.CreatedDate           // FIXED
            };
        }

        public async Task<StudentDto> CreateStudent(CreateStudentDTO student)
        {
            var stud = new Student()
            {
                Name = student.Name,
                Age = student.Age,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,       //  added
                CreatedDate = DateTime.UtcNow            //  correct place
            };

            stud = await _repository.CreateStudent(stud);

            return new StudentDto
            {
                Id = stud.Id,
                Name = stud.Name,
                Email = stud.Email,
                Age = stud.Age,
                PhoneNumber = stud.PhoneNumber,
                CreatedDate = stud.CreatedDate
            };
        }

        public async Task<StudentDto?> UpdateStudent(int id, UpdateStudentDto student)
        {
            var existing = await _repository.GetStudentById(id);

            if (existing == null)
                return null;

            //  Update logic moved to service
            existing.Name = student.Name;
            existing.Email = student.Email;
            existing.Age = student.Age;
            existing.PhoneNumber = student.PhoneNumber;

            //  DO NOT touch CreatedDate

            await _repository.SaveChangesAsync();

            return new StudentDto
            {
                Id = existing.Id,
                Name = existing.Name,
                Email = existing.Email,
                Age = existing.Age,
                PhoneNumber = existing.PhoneNumber,
                CreatedDate = existing.CreatedDate
            };
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var existing= await _repository.GetStudentById(id); // fixed

            if (existing == null)
                return false;

            return await _repository.DeleteStudent(id);
        }
    }
}