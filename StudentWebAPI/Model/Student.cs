using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Range(5, 30, ErrorMessage = "Enter the value within range")]
        public int Age { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
