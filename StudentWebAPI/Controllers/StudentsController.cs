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
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        // GET: api/students/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
                return NotFound($"Student with Id {id} not found");

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStudent = await _studentService.CreateStudent(dto);

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = createdStudent.Id },
                createdStudent
            );
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
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
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id <= 0) return BadRequest();

            var result = await _studentService.DeleteStudent(id);

            if (!result)
                return NotFound($"Student with Id {id} not found");

            return NoContent();
        }
    }
}
