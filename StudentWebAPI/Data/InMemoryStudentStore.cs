using StudentWebAPI.Model;

namespace StudentWebAPI.Data
{
    public class InMemoryStudentStore
    {
        public List<Student> Students { get; } = new()
        {
            new Student
            {
                Id = 1,
                Name = "Sai",
                Age = 25
            },
            new Student
            {
                Id = 2,
                Name = "Ravi",
                Age = 23,
                Email = "ravi@test.com"
            }
        };
    }
}
