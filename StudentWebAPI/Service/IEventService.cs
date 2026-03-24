using StudentWebAPI.DTO;

namespace StudentWebAPI.Service
{
    public interface IEventService
    {
        Task<IEnumerable<EventResponseDTO>> GetAllEventsAsync();

        Task<EventResponseDTO?> GetEventByIdAsync(int id);

        Task<EventResponseDTO> CreateEventAsync(CreateEventDTO eventDTO);

        Task<EventResponseDTO?> UpdateEventAsync(int id, UpdateEventDTO eventDTO);

        Task<bool> DeleteEventAsync(int id);
    }
}
