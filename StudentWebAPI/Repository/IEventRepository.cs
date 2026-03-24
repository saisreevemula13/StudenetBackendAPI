using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event> AddEventAsync(Event eventEntity);
        Task<bool> SaveChangesAsync();

        Task<bool> DeleteEventAsync(int id);
    }
}
