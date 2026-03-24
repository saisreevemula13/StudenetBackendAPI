using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Service
{
    public interface IRegistrationService
    {
        Task<IEnumerable<RegistrationResponseDTO>> GetAllRegistrationAsync();
        Task<RegistrationResponseDTO?> GetRegistrationByIdAsync(int id);
        Task<RegistrationResponseDTO?> CreateRegistrationAsync(RegistrationCreateDTO register);
        Task<RegistrationResponseDTO?> UpdateRegistrationAsync(int id, RegistrationUpdateDTO registration);
        Task<bool> DeleteRegistrationAsync(int id);
    }
}
