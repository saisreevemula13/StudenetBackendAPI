using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentWebAPI.Data;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using System.Linq;

namespace StudentWebAPI.Repository
{
    public class RegisterRepository : IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;
        public RegisterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Registration> CreateRegistrationsAsync(Registration registration)
        {
            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }


        public async Task<bool> DeleteRegistrationByIdAsync(int id)
        {
            var existing = await _context.Registrations.FirstOrDefaultAsync(x=>x.Id==id);

            if (existing == null)
                return false;

            _context.Registrations.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Registration>> GetAllRegistrationsAsync(string? FilterOn = null, string? FilterQuery = null,
            string? sortBy = null, bool isAscending = true,
           int PageNumber = 1, int PageSize = 3)
        {
            var studentList = _context.Registrations.Include(r => r.Student)
                .Include(r => r.Event).AsQueryable();

            //Filtering based on StudentId, EventId, studentName
            if (!string.IsNullOrEmpty(FilterOn) && !string.IsNullOrEmpty(FilterQuery))
            {
                if (string.Equals(FilterOn, "StudentName", StringComparison.OrdinalIgnoreCase))
                {
                    studentList = studentList.Where(x => x.Student.Name.Contains(FilterQuery));
                }

                else if (string.Equals(FilterOn, "StudentId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(FilterQuery, out int studentId))
                    {
                        studentList = studentList.Where(x => x.StudentId == studentId);
                    }

                }
                else if (string.Equals(FilterOn, "EventId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(FilterQuery, out int eventId))
                    {
                        studentList = studentList.Where(x => x.EventId == eventId);
                    }
                }
            }
                //sorting
                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (string.Equals(sortBy, "StudentId", StringComparison.OrdinalIgnoreCase))
                    {
                        studentList = isAscending ? studentList.OrderBy(x => x.StudentId) : studentList.OrderByDescending(x => x.StudentId);
                    }
                }
                //Pagination
                var skipResults=(PageNumber-1)*PageSize;

           return await studentList.Skip(skipResults).Take(PageSize).ToListAsync();  
        }

        public async Task<Registration?> GetRegistrationsByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r=>r.Event)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }


        public async Task<bool> RegistrationExistsAsync(int studentId, int eventId)
        {
            return await _context.Registrations
                .AnyAsync(x => x.StudentId == studentId && x.EventId == eventId);
        }

        public async Task<int> GetRegistrationCountByEventIdAsync(int eventId)
        {
            return await _context.Registrations
                .CountAsync(r=>r.EventId == eventId);
        }
        public async Task<Registration?> GetRegistrationWithDetailsByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
