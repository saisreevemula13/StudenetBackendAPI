using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public StudentService(
            IStudentRepository repo,
            ILogger<StudentService> logger,
            IMapper mapper,
            IMemoryCache cache)
        {
            _repository = repo;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<StudentDto>> GetAllStudents(
            string? FilterOn = null,
            string? FilterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int PageNumber = 1,
            int PageSize = 1000)
        {
            var studentList = await _repository.GetAllStudents(
                FilterOn,
                FilterQuery,
                sortBy,
                isAscending,
                PageNumber,
                PageSize);

            return _mapper.Map<List<StudentDto>>(studentList);
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            string cacheKey = $"student_{id}";

            // Step-1: Check cache
            if (_cache.TryGetValue(cacheKey, out StudentDto cachedStudent))
            {
                _logger.LogInformation("Student fetched from cache with Id {StudentId}", id);
                return cachedStudent;
            }

            _logger.LogInformation("Fetching student from DB with Id {StudentId}", id);

            // Step-2: Fetch from DB
            var stud = await _repository.GetStudentById(id);

            if (stud == null)
            {
                _logger.LogWarning("Student not found with Id {StudentId}", id);
                throw new NotFoundException($"Student with {id} is not found");
            }

            var studentDto = _mapper.Map<StudentDto>(stud);

            // Step-3: Store in cache
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, studentDto, cacheOptions);

            // Step-4: Return data
            return studentDto;
        }

        public async Task<StudentDto> CreateStudent(CreateStudentDTO dto)
        {
            _logger.LogInformation("Creating student with Email {Email}", dto.Email);

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

            var stud = _mapper.Map<Student>(dto);

            try
            {
                stud = await _repository.CreateStudent(stud);
            }
            catch (DbUpdateException)
            {
                throw new BadRequestException("Duplicate data detected");
            }

            _logger.LogInformation("Student created successfully with Id {Id}", stud.Id);

            return _mapper.Map<StudentDto>(stud);
        }

        public async Task<StudentDto?> UpdateStudent(int id, UpdateStudentDto student)
        {
            var existing = await _repository.GetStudentById(id);

            if (existing == null)
                throw new NotFoundException($"Student with {id} is not found");

            _mapper.Map(student, existing);

            await _repository.SaveChangesAsync();

            // Remove old cache after update
            _cache.Remove($"student_{id}");

            return _mapper.Map<StudentDto>(existing);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var existing = await _repository.GetStudentById(id);

            if (existing == null)
                return false;

            var result = await _repository.DeleteStudent(id);

            // Remove cache after delete
            _cache.Remove($"student_{id}");

            return result;
        }
    }
}
