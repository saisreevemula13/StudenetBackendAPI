using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.Model;
using StudentWebAPI.Service;

namespace StudentWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
            private readonly StudentService _service;

            public StudentsController(StudentService service)
            {
                _service = service;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                return Ok(_service.GetAll());
            }

        [HttpPost]
        public IActionResult Test(StudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Valid");
        }

        [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var result = _service.Delete(id);
                if (!result) return NotFound();

                return NoContent();
            }
        }
    }
