using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.Model
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Registration>? Registrations { get; set; }
    }
}
