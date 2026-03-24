using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;

namespace StudentWebAPI.Service
{
    public class RegistrationService: IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<IEnumerable<RegistrationResponseDTO>> GetAllRegistrationAsync()
        {
            var registrations = await _registrationRepository.GetAllRegistrationsAsync();

            return registrations.Select(r => new RegistrationResponseDTO
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student?.Name,
                EventId = r.EventId,
                EventTitle = r.Event?.Title,
                RegisteredOn = r.RegisteredOn
            });
        }

        public async Task<RegistrationResponseDTO?> GetRegistrationByIdAsync(int id)
        {
            var registration = await _registrationRepository.GetRegistrationsByIdAsync(id);

            if (registration == null)
                return null;
           
            return new RegistrationResponseDTO
            {
                Id= registration.Id,
                StudentId=registration.StudentId,
                StudentName=registration.Student?.Name,
                EventId=registration.EventId,
                RegisteredOn=registration.RegisteredOn,
                EventTitle=registration.Event?.Title
            };
        }

        public async Task<RegistrationResponseDTO> CreateRegistrationAsync(RegistrationCreateDTO dto)
        {
            var exists = await _registrationRepository.RegistrationExistsAsync(dto.StudentId, dto.EventId);

            if (exists)
               return null;

            var registration = new Registration
            {
                StudentId = dto.StudentId,
                EventId = dto.EventId
            };

            var created = await _registrationRepository.CreateRegistrationsAsync(registration);

            return new RegistrationResponseDTO
            {
                Id = created.Id,
                StudentId = created.StudentId,
                EventId = created.EventId,
                RegisteredOn = created.RegisteredOn,
                StudentName= created.Student?.Name,
                EventTitle= created.Event?.Title
            };
        }

        public async Task<RegistrationResponseDTO?> UpdateRegistrationAsync(int id, RegistrationUpdateDTO registration)
        {
            var existingRegistration = await _registrationRepository.GetRegistrationsByIdAsync(id);

            if (existingRegistration == null)
                return null;

            existingRegistration.StudentId = registration.StudentId;
            existingRegistration.EventId = registration.EventId;
          
            await _registrationRepository.SaveChangesAsync();

            return new RegistrationResponseDTO
            {
                Id = existingRegistration.Id,
                StudentId = existingRegistration.StudentId,
                EventId = existingRegistration.EventId,
                RegisteredOn = existingRegistration.RegisteredOn
            };
        }

        public async Task<bool> DeleteRegistrationAsync(int id)
        {
            return await _registrationRepository.DeleteRegistrationByIdAsync(id);
        }
    }
}
