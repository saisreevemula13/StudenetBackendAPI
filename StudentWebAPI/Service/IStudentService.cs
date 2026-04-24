using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Service
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllStudents(string? filterOn=null, string? filterQuery=null,string? sortBy=null, bool isAscending=true
            ,int PageNumber=1, int PageSize=1000);
        Task<StudentDto?> GetStudentById(int id);
        Task<StudentDto?> CreateStudent(CreateStudentDTO dto);
        Task<StudentDto?> UpdateStudent(int id, UpdateStudentDto student);
        Task<bool> DeleteStudent(int id);
    }
}
