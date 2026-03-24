namespace StudentWebAPI.Model
{
    public class Registration
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public DateTime RegisteredOn { get; set; }= DateTime.UtcNow;
    }
}
