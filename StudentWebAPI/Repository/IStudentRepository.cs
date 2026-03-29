using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudents();

        Task<Student?> GetStudentById(int id);

        Task<Student> CreateStudent(Student student);

        Task<bool> DeleteStudent(int id);

        Task<bool> SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByPhoneAsync(string phone);
    }
}
