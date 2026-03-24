using Microsoft.EntityFrameworkCore;
using StudentWebAPI.Data;
using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public EventRepository(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<Event> AddEventAsync(Event eventEntity)
        {
           await _dbContext.Events.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();
            return eventEntity;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _dbContext.Events.ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync()>0;
        }
        public async Task<bool> DeleteEventAsync(int id)
        {
            var existing = await _dbContext.Events.FirstOrDefaultAsync(x=>x.Id==id);

            if (existing == null)
                return false;

            _dbContext.Events.Remove(existing);

            await  _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
