using Microsoft.EntityFrameworkCore;
using StudentWebAPI.DTO;
using StudentWebAPI.Exceptions;
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

        public async Task<StudentDto> CreateStudent(CreateStudentDTO dto)
        {
            if (await _repository.ExistsByEmailAsync(dto.Email))
                throw new BadRequestException("Email already exists");

            if (await _repository.ExistsByPhoneAsync(dto.PhoneNumber))
                throw new BadRequestException("Phone number already exists");


        var stud = new Student()
        {
            Name = dto.Name,
            Age = dto.Age,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,       //  added
            CreatedDate = DateTime.UtcNow            //  correct place
        };
            try
            {
                stud = await _repository.CreateStudent(stud);
            }
            catch (DbUpdateException)
            {
                throw new BadRequestException("Duplicate data detected");
            }

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
            existing.Name = student.Name?? existing.Name;
            existing.Email = student.Email?? existing.Email;
            existing.Age = student.Age?? existing.Age;
            existing.PhoneNumber = student.PhoneNumber?? existing.PhoneNumber;

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