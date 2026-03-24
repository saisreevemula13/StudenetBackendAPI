using Microsoft.EntityFrameworkCore;
using StudentWebAPI.Data;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;

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

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
           return await _context.Registrations.
                Include(r=>r.Student)
                .Include(r=>r.Event)
                .ToListAsync();  
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
    }
}
