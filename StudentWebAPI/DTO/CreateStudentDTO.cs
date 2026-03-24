using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.DTO
{
    public class CreateStudentDTO
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }
}
