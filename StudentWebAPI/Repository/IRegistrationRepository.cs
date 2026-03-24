using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetAllRegistrationsAsync();
        Task<Registration?> GetRegistrationsByIdAsync(int id);
        Task<Registration> CreateRegistrationsAsync(Registration registration);
        Task<bool> SaveChangesAsync();
        Task<bool> DeleteRegistrationByIdAsync(int id);
        Task<bool> RegistrationExistsAsync(int studentId, int eventId);
    }
}
