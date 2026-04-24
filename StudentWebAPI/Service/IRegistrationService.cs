using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Service
{
    public interface IRegistrationService
    {
        Task<List<RegistrationResponseDTO>> GetAllRegistrationAsync(string? FilterOn=null, string? FilterQuery=null,
              string? sortBy=null, bool isAscending=true,
              int PageNumber = 1, int PageSize = 3);
        Task<RegistrationResponseDTO?> GetRegistrationByIdAsync(int id);
        Task<RegistrationResponseDTO?> CreateRegistrationAsync(RegistrationCreateDTO register);
        Task<RegistrationResponseDTO> UpdateRegistrationAsync(int id, RegistrationUpdateDTO registration);
        Task DeleteRegistrationAsync(int id);
    }
}
