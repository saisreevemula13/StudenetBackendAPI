namespace StudentWebAPI.DTO
{
    public class UpdateEventDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
    }
}
