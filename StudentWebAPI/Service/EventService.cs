using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;

namespace StudentWebAPI.Service
{
    public class EventServicecs : IEventService
    {
        private readonly IEventRepository _repo;

        public EventServicecs(IEventRepository repo)
        {
            _repo = repo;
        }
        public async Task<EventResponseDTO> CreateEventAsync(CreateEventDTO eventDTO)
        {
            //dto to entity conversion
            var eventt = new Event
            {
                Capacity = eventDTO.Capacity,
                EventDate = eventDTO.EventDate,
                Description = eventDTO.Description,
                Title = eventDTO.Title,
            };

            var created=await _repo.AddEventAsync(eventt);

           return new EventResponseDTO
            {
                Id= created.Id,
                EventDate = created.EventDate,
                Description = created.Description,
                Title = created.Title,
                Capacity = created.Capacity
            };
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
           return await _repo.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<EventResponseDTO>> GetAllEventsAsync()
        {
            var eventss=await _repo.GetAllEventsAsync();

            var eventssList=new List<EventResponseDTO>();

            foreach(var e in eventss)
            {
                var eventsdto = new EventResponseDTO
                {
                    Id = e.Id,
                    EventDate = e.EventDate,
                    Capacity = e.Capacity,
                    Description = e.Description,
                    Title = e.Title
                };
                eventssList.Add(eventsdto);
            }
            return eventssList;
          }

        public async Task<EventResponseDTO?> GetEventByIdAsync(int id)
        {
            var eventt=await _repo.GetEventByIdAsync(id);

            if (eventt == null) return null;

            return new EventResponseDTO
            {
                Id = eventt.Id,
                EventDate = eventt.EventDate,
                Capacity = eventt.Capacity,
                Description = eventt.Description,
                Title = eventt.Title
            };

        }

        public async Task<EventResponseDTO?> UpdateEventAsync(int id, UpdateEventDTO eventDTO)
        {
            var existing=await _repo.GetEventByIdAsync(id);

            if (existing == null)
                return null;

            existing.Capacity = eventDTO.Capacity;
            existing.Description = eventDTO.Description;
            existing.Title = eventDTO.Title;

            await _repo.SaveChangesAsync();

            return (new EventResponseDTO
            {
                Id=existing.Id,
                Capacity = existing.Capacity,
                EventDate = existing.EventDate,
                Description = existing.Description,
                Title = existing.Title
            });
        }
    }
}
