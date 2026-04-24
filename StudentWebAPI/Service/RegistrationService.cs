using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.DTO;
using StudentWebAPI.Exceptions;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;

namespace StudentWebAPI.Service
{
    public class RegistrationService: IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ILogger<RegistrationService> _logger;
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public RegistrationService(IRegistrationRepository registrationRepository,ILogger<RegistrationService> logger,
            IEventRepository eventRepo, IMapper mapper)
        {
            _registrationRepository = registrationRepository;
            _logger = logger;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<List<RegistrationResponseDTO>> GetAllRegistrationAsync(string? FilterOn = null, string? FilterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int PageNumber = 1, int PageSize = 3)
        {
            _logger.LogInformation("Fetching all registrations");

            var registrations = await _registrationRepository.GetAllRegistrationsAsync(FilterOn,FilterQuery,sortBy,isAscending);

            _logger.LogInformation("fetched {count} registrations",registrations.Count());

            //return registrations.Select(r => new RegistrationResponseDTO
            //{
            //    Id = r.Id,
            //    StudentId = r.StudentId,
            //    StudentName = r.Student?.Name,
            //    EventId = r.EventId,
            //    EventTitle = r.Event?.Title,
            //    RegisteredOn = r.RegisteredOn
            //});
            return _mapper.Map<List<RegistrationResponseDTO>>(registrations);
        }

        public async Task<RegistrationResponseDTO?> GetRegistrationByIdAsync(int id)
        {

            _logger.LogInformation("Fetching registration with Id: {Id}", id);

            var registration = await _registrationRepository.GetRegistrationsByIdAsync(id);

            if (registration == null)
            {
                _logger.LogWarning("Registration not found for Id: {Id}", id);
                throw new NotFoundException("registration not Found");
            }
           
            //return new RegistrationResponseDTO
            //{
            //    Id= registration.Id,
            //    StudentId=registration.StudentId,
            //    StudentName=registration.Student?.Name,
            //    EventId=registration.EventId,
            //    RegisteredOn=registration.RegisteredOn,
            //    EventTitle=registration.Event?.Title
            //};
            return _mapper.Map<RegistrationResponseDTO>(registration);
        }

        public async Task<RegistrationResponseDTO> CreateRegistrationAsync(RegistrationCreateDTO dto)
        {
            _logger.LogInformation("registration request started for StudentId : {StudentId}, EventId :{EventId}", dto.StudentId, dto.EventId);
            
            //Event exists

            var eventEntity=await _eventRepo.GetEventByIdAsync(dto.EventId);

            if(eventEntity==null)
            {
                throw new NotFoundException("Event not found");
            }
            //Duplicate check first
            var exists = await _registrationRepository.RegistrationExistsAsync(dto.StudentId, dto.EventId);

            if (exists)
            {
                _logger.LogWarning("Duplicate registration attempt for attempt for StudentId: {StudentId}, EventId:{EventId}", dto.StudentId, dto.EventId);
                throw new BadRequestException("Student already registered for this event");
            }
            //capacity check
            var currentCount = await _registrationRepository.GetRegistrationCountByEventIdAsync(dto.EventId);
            
            if (currentCount >= eventEntity.Capacity)
                throw new BadRequestException("Event capacity is full");
            
            //save
            //var registration = new Registration
            //{
            //    StudentId = dto.StudentId,
            //    EventId = dto.EventId
            //};
            var registration=_mapper.Map<Registration>(dto);

            _logger.LogInformation("Saving registration to database");

            var created = await _registrationRepository.CreateRegistrationsAsync(registration);
            var fullData = await _registrationRepository.GetRegistrationWithDetailsByIdAsync(created.Id);


            _logger.LogInformation("Registration created successfully with Id: {RegistrationId}", created.Id);

            //return new RegistrationResponseDTO
            //{
            //    Id = fullData.Id,
            //    StudentId = fullData.StudentId,
            //    EventId = fullData.EventId,
            //    RegisteredOn = fullData.RegisteredOn,
            //    StudentName= fullData.Student?.Name,
            //    EventTitle= fullData.Event?.Title
            //};
            return _mapper.Map<RegistrationResponseDTO>(fullData);
        }

        public async Task<RegistrationResponseDTO> UpdateRegistrationAsync(int id, RegistrationUpdateDTO registration)
        {
            _logger.LogInformation("Updating registration Id: {Id}", id);

            var existingRegistration = await _registrationRepository.GetRegistrationsByIdAsync(id);

            if (existingRegistration == null)
            {
                _logger.LogWarning("Registration not found for update Id: {Id}", id);
                throw new NotFoundException("registration is not found");
            }
            //event exists

            var eventEntity = await _eventRepo.GetEventByIdAsync(registration.EventId);
            
            if (eventEntity == null)
            {
                throw new NotFoundException("Event not Found");
            }
            //Duplicate check
            var exists = await _registrationRepository.RegistrationExistsAsync(registration.StudentId, registration.EventId);
            if (exists &&
        (existingRegistration.StudentId != registration.StudentId ||
         existingRegistration.EventId != registration.EventId))
            {
                throw new BadRequestException("Duplicate registration");
            }
            // WCapacity check
            var count = await _registrationRepository
                .GetRegistrationCountByEventIdAsync(registration.EventId);

            if (count >= eventEntity.Capacity)
                throw new BadRequestException("Event capacity full");


            //existingRegistration.StudentId = registration.StudentId;
            //existingRegistration.EventId = registration.EventId;
            _mapper.Map(registration, existingRegistration);
          
            await _registrationRepository.SaveChangesAsync();

            //return new RegistrationResponseDTO
            //{
            //    Id = existingRegistration.Id,
            //    StudentId = existingRegistration.StudentId,
            //    EventId = existingRegistration.EventId,
            //    RegisteredOn = existingRegistration.RegisteredOn
            //};
            return _mapper.Map<RegistrationResponseDTO>(existingRegistration);
        }

        public async Task DeleteRegistrationAsync(int id)
        {
            _logger.LogInformation("Deleting registration Id :{Id}", id);

            var result=await _registrationRepository.DeleteRegistrationByIdAsync(id);

            if (!result)
            {
                _logger.LogWarning("Delete failed. registration not found Id: {Id}", id);
                throw new NotFoundException("registration not found"); 
            }
            _logger.LogInformation("Registration deleted successfully Id: {Id}");

        }

    }
}
