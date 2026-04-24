using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;
using StudentWebAPI.Repository;
using StudentWebAPI.Service;

namespace StudentWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int PageNumber=1, [FromQuery] int PageSize=1000)
        {
            var students = await _studentService.GetAllStudents(FilterOn, FilterQuery, sortBy, isAscending ?? true,PageNumber, PageSize);
            return Ok(students);
        }

        // GET: api/students/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var student = await _studentService.GetStudentById(id);
            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO dto)
        {
            var result=await _studentService.CreateStudent(dto);

            return Ok(result);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] UpdateStudentDto dto)
        {
            if(id<=0) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStudent = await _studentService.UpdateStudent(id, dto);

            if (updatedStudent == null)
                return NotFound($"Student with Id {id} not found");

            return Ok(updatedStudent);
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (id <= 0) return BadRequest();

            var result = await _studentService.DeleteStudent(id);

            if (!result)
                return NotFound($"Student with Id {id} not found");

            return NoContent();
        }
    }
}
