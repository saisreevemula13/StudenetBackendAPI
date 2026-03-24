using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudents();
        Task<StudentDto?> GetStudentById(int id);
        Task<StudentDto?> CreateStudent(CreateStudentDTO dto);
        Task<StudentDto?> UpdateStudent(int id, UpdateStudentDto student);
        Task<bool> DeleteStudent(int id);
    }
}
