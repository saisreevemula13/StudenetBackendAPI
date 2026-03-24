using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.DTO;
using StudentWebAPI.Service;

namespace StudentWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventDetails()
        {
            var eventLists = await _eventService.GetAllEventsAsync();
            return Ok(eventLists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var eventss = await _eventService.GetEventByIdAsync(id);

            if (eventss == null)
                return NotFound();

            return Ok(eventss);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO eventdto)
        {
            var eventss = await _eventService.CreateEventAsync(eventdto);

            return CreatedAtAction(nameof(GetEventById),
                new { id = eventss.Id }, eventss);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventDTO eventDTO)
        {
            if (id <= 0 || eventDTO == null)
                return BadRequest();

            var eventss = await _eventService.UpdateEventAsync(id, eventDTO);

            if (eventss == null)
                return NotFound();

            return Ok(eventss);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (id <= 0)
                return BadRequest();

            var eventts = await _eventService.DeleteEventAsync(id);

            if (!eventts)
                return NotFound();

            return NoContent();
        }
    }
}
