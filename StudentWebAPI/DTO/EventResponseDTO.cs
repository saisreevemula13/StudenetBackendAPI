namespace StudentWebAPI.DTO
{
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
    }
}
