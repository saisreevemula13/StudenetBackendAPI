using StudentWebAPI.Model;

namespace StudentWebAPI.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudents(string? filterOn=null, string? filterQuery=null,
            string? sortBy=null, bool isAscending = true, int PageNumber = 1, int PageSize = 1000);

        Task<Student?> GetStudentById(int id);

        Task<Student> CreateStudent(Student student);

        Task<bool> DeleteStudent(int id);

        Task<bool> SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByPhoneAsync(string phone);
    }
}
