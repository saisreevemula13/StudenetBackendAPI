// StudentRepository
using Microsoft.EntityFrameworkCore;
using StudentWebAPI.Data;
using StudentWebAPI.Model;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StudentWebAPI.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents(string? filterOn=null, string? filterQuery=null,
            string? sortBy=null, bool isAscending = true, int PageNumber = 1, int PageSize = 1000)
        {
            //filtering used AsQuerable as in the DB side we do get data and the apply filter on it.
            var studentList = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (string.Equals(filterOn,"Name", StringComparison.OrdinalIgnoreCase))
                {
                    studentList = studentList.Where(x => x.Name.Contains(filterQuery));
                }

                else if (string.Equals(filterOn,"Email", StringComparison.OrdinalIgnoreCase))
                {
                    studentList = studentList.Where(x => x.Email.Contains(filterQuery));
                }
            }
            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    studentList = isAscending ? studentList.OrderBy(x => x.Name):studentList.OrderByDescending(x => x.Name);    
                }
                else if (string.Equals(sortBy, "Age", StringComparison.OrdinalIgnoreCase))
                {
                    studentList = isAscending ? studentList.OrderBy(x => x.Age) : studentList.OrderByDescending(x => x.Age);
                }
            }

            //Pagination
            var skipResults = (PageNumber = 1) * PageSize;
            return await studentList.Skip(skipResults).Take(PageSize).ToListAsync();
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student> CreateStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Students.AnyAsync(x => x.Email == email);
        }

        public Task<bool> ExistsByPhoneAsync(string phone)
        {
            return _context.Students.AnyAsync(x=>x.PhoneNumber == phone);
        }

        
    }
}