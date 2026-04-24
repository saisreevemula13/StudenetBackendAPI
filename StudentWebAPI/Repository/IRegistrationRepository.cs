using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public interface IRegistrationRepository
    {
        Task<List<Registration>> GetAllRegistrationsAsync(string? FilterOn = null, string? FilterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int PageNumber = 1, int PageSize = 3);
        Task<Registration?> GetRegistrationsByIdAsync(int id);
        Task<Registration> CreateRegistrationsAsync(Registration registration);
        Task<bool> SaveChangesAsync();
        Task<int> GetRegistrationCountByEventIdAsync(int eventId);
        Task<bool> DeleteRegistrationByIdAsync(int id);
        Task<bool> RegistrationExistsAsync(int studentId, int eventId);
        Task<Registration?> GetRegistrationWithDetailsByIdAsync(int id);


    }
}
