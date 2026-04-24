using AutoMapper;
using StudentWebAPI.DTO;
using StudentWebAPI.Exceptions;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;

namespace StudentWebAPI.Service
{
    public class EventServicecs : IEventService
    {
        private readonly IEventRepository _repo;
        private readonly ILogger<EventServicecs> _logger;
        private readonly IMapper _mapper;

        public EventServicecs(IEventRepository repo, ILogger<EventServicecs> logger
            ,IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<EventResponseDTO> CreateEventAsync(CreateEventDTO eventDTO)
        {
            _logger.LogInformation("The event is getting created");
            //dto to entity conversion
            //var eventt = new Event
            //{
            //    Capacity = eventDTO.Capacity,
            //    EventDate = eventDTO.EventDate,
            //    Description = eventDTO.Description,
            //    Title = eventDTO.Title,
            //};
            var eventt=_mapper.Map<Event>(eventDTO);

            var created=await _repo.AddEventAsync(eventt);
            _logger.LogInformation("New event is been created");
           //return new EventResponseDTO
           // {
           //     Id= created.Id,
           //     EventDate = created.EventDate,
           //     Description = created.Description,
           //     Title = created.Title,
           //     Capacity = created.Capacity
           // };
           return _mapper.Map<EventResponseDTO>(created);
        }

        public async Task DeleteEventAsync(int id)
        {
            _logger.LogInformation("Checking id for the deletion");
            var result=await _repo.DeleteEventAsync(id);

            if (!result)
            {
                _logger.LogWarning("Event with Id {Id} not found for deletion", id);
                throw new NotFoundException($"Event with id {id} not found");
            }

            _logger.LogInformation("Event deleted successfully Id: {Id}", id);
        }

        public async Task<IEnumerable<EventResponseDTO>> GetAllEventsAsync()
        {
            _logger.LogInformation("Fetching all Events");

            var eventss=await _repo.GetAllEventsAsync();

            //var eventssList=new List<EventResponseDTO>();

            //foreach(var e in eventss)
            //{
            //    var eventsdto = new EventResponseDTO
            //    {
            //        Id = e.Id,
            //        EventDate = e.EventDate,
            //        Capacity = e.Capacity,
            //        Description = e.Description,
            //        Title = e.Title
            //    };
            //    eventssList.Add(eventsdto);
            //}
            
            return _mapper.Map<IEnumerable<EventResponseDTO>>(eventss);
          }

        public async Task<EventResponseDTO> GetEventByIdAsync(int id)
        {
            _logger.LogInformation("Fetching event details for Id: {Id}", id);

            var eventt=await _repo.GetEventByIdAsync(id);

            if (eventt == null)
            {
                _logger.LogWarning("eventid is not present throwing exception");
                throw new NotFoundException($"Event id {id} is not found");
            }

            _logger.LogInformation("Returning the evnt details");

            //return new EventResponseDTO
            //{
            //    Id = eventt.Id,
            //    EventDate = eventt.EventDate,
            //    Capacity = eventt.Capacity,
            //    Description = eventt.Description,
            //    Title = eventt.Title
            //};
            //
            return  _mapper.Map<EventResponseDTO>(eventt);
        }

        public async Task<EventResponseDTO> UpdateEventAsync(int id, UpdateEventDTO eventDTO)
        {
            _logger.LogInformation("updating event id: {Id}", id);
            var existing=await _repo.GetEventByIdAsync(id);

            if (existing==null)
            {
                _logger.LogWarning("Event with event id {id} is not present", id);
                throw new NotFoundException($"No event with event id {id}");
            }

            _mapper.Map(eventDTO, existing);

            await _repo.SaveChangesAsync();

            _logger.LogInformation($"Updated event with id:{id} {existing.Id}");

            //return (new EventResponseDTO
            //{
            //    Id=existing.Id,
            //    Capacity = existing.Capacity,
            //    EventDate = existing.EventDate,
            //    Description = existing.Description,
            //    Title = existing.Title
            //});
            return _mapper.Map<EventResponseDTO>(existing);
        }

    }
}
