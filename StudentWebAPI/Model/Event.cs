using System.ComponentModel.DataAnnotations;

namespace StudentWebAPI.Model
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime EventDate {  get; set; }

        public int Capacity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        //navigation property
        public ICollection<Registration>? Registrations { get; set; }

    }
}
