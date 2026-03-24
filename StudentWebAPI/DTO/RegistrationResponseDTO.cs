namespace StudentWebAPI.DTO
{
    public class RegistrationResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
