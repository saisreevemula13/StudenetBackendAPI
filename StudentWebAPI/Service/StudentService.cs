using AutoMapper;
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
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository repo, ILogger<StudentService> logger,
            IMapper mapper)
        {
            _repository = repo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> GetAllStudents(string? FilterOn=null, string? FilterQuery=null,
           string? sortBy=null, bool isAscending = true, int PageNumber = 1, int PageSize = 1000)
        {
            var students = await _repository.GetAllStudents(FilterOn, FilterQuery,sortBy, isAscending,PageNumber, PageSize);

            //return students.Select(stud => new StudentDto
            //{
            //    Id = stud.Id,
            //    Name = stud.Name,
            //    Email = stud.Email,
            //    Age = stud.Age,
            //    PhoneNumber = stud.PhoneNumber,          // added
            //    CreatedDate = stud.CreatedDate           //FIXED
            //});
            return _mapper.Map<List<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            _logger.LogInformation("Fetching student with Id {StudentId}", id);

            var stud = await _repository.GetStudentById(id);

            if (stud == null)
            {
                _logger.LogWarning("Student not found with Id {StudentId}", id);

                throw new NotFoundException($"Student with {id} is not found");
            }
            //return new StudentDto
            //{
            //    Id = stud.Id,
            //    Name = stud.Name,
            //    Email = stud.Email,
            //    Age = stud.Age,
            //    PhoneNumber = stud.PhoneNumber,          // added
            //    CreatedDate = stud.CreatedDate           // FIXED
            //};
            return _mapper.Map<StudentDto>(stud);
        }

        public async Task<StudentDto> CreateStudent(CreateStudentDTO dto)
        {
            _logger.LogInformation("creating student with Email {Email}", dto.Email);

            if (await _repository.ExistsByEmailAsync(dto.Email))
            {
                _logger.LogWarning("Duplicate email detected: {Email}", dto.Email);
                throw new BadRequestException("Email already exists");
            }

            if (await _repository.ExistsByPhoneAsync(dto.PhoneNumber))
            {
                _logger.LogWarning("Duplicate phone number: {Phone}", dto.PhoneNumber);
                throw new BadRequestException("Phone number already exists");
            }

        //var stud = new Student()
        //{
        //    Name = dto.Name,
        //    Age = dto.Age,
        //    Email = dto.Email,
        //    PhoneNumber = dto.PhoneNumber,       //  added
        //    CreatedDate = DateTime.UtcNow            //  correct place
        //};
        var stud=_mapper.Map<Student>(dto);

            try
            {
                stud = await _repository.CreateStudent(stud);
            }
            catch (DbUpdateException)
            {
                throw new BadRequestException("Duplicate data detected");
            }
            _logger.LogInformation("Student created successfully with Id {Id}", stud.Id);
            //return new StudentDto
            //{
            //    Id = stud.Id,
            //    Name = stud.Name,
            //    Email = stud.Email,
            //    Age = stud.Age,
            //    PhoneNumber = stud.PhoneNumber,
            //    CreatedDate = stud.CreatedDate
            //};
            return _mapper.Map<StudentDto>(stud);
        }
        public async Task<StudentDto?> UpdateStudent(int id, UpdateStudentDto student)
        {
            var existing = await _repository.GetStudentById(id);

            if (existing == null)
                throw new NotFoundException($"Student with {id} is not found");

            //  Update logic moved to service
            _mapper.Map(student, existing);
            //  DO NOT touch CreatedDate

            await _repository.SaveChangesAsync();

            //return new StudentDto
            //{
            //    Id = existing.Id,
            //    Name = existing.Name,
            //    Email = existing.Email,
            //    Age = existing.Age,
            //    PhoneNumber = existing.PhoneNumber,
            //    CreatedDate = existing.CreatedDate
            //};
            return _mapper.Map<StudentDto>(existing);
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