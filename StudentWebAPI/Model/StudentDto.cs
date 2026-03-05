using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.Model
{
    public class StudentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(5, 30)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
