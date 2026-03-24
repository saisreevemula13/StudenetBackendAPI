namespace StudentWebAPI.DTO
{
    public class RegistrationCreateDTO
    {
        //we use this DTO when student registers for an event
        public int StudentId { get; set; }
        public int EventId { get; set; }
    }
}
