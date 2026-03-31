using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.DTO
{
    public class UpdateStudentDto
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }

       public string? PhoneNumber { get; set; }
    }
}
