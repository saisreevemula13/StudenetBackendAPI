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
        public async Task<IActionResult> GetAll()
        {
            var data = await _services.GetAllRegistrationAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationsById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var registrations = _services.GetRegistrationByIdAsync(id);

            if(registrations == null) 
                return NotFound();

            return Ok(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistration(RegistrationCreateDTO registration)
        {
            var result = await _services.CreateRegistrationAsync(registration);

            if (result == null)
                return BadRequest("Already registered");

            return CreatedAtAction(nameof(GetRegistrationsById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, RegistrationUpdateDTO register)
        {
            if (id <= 0) return BadRequest();

            var result= await _services.UpdateRegistrationAsync(id, register);
            
            if (result==null)
                return NotFound();

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            if(id<=0) return BadRequest();
            
            var result=await _services.DeleteRegistrationAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}