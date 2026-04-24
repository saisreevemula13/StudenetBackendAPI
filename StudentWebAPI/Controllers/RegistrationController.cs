using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using StudentWebAPI.Service;

namespace StudentWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _services;
        public RegistrationController(IRegistrationService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int  PageNumber= 1, [FromQuery] int PageSize=3)
        {
            var data = await _services.GetAllRegistrationAsync(FilterOn,FilterQuery,sortBy, isAscending ?? true,PageNumber,PageSize);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRegistrationsById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Id");
            }

            var registrations = await _services.GetRegistrationByIdAsync(id);

            return Ok(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistration([FromBody] RegistrationCreateDTO registration)
        {
            var result = await _services.CreateRegistrationAsync(registration);

            return CreatedAtAction(nameof(GetRegistrationsById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRegistration([FromRoute] int id,[FromBody] RegistrationUpdateDTO register)
        {
            if (id <= 0) return BadRequest("Invalid Id");

            var result= await _services.UpdateRegistrationAsync(id, register);

            return Ok(result);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRegistration([FromBody] int id)
        {
            if(id<=0) return BadRequest("Invalid Id");
            
            await _services.DeleteRegistrationAsync(id);

            return NoContent();
        }
    }
}