using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
