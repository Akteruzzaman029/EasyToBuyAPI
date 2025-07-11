using Core.Model;
using Core.ModelDto.Student;

namespace Infrastructure.IRepository;

public interface IStudentRepository
{
    Task<PaginatedListModel<StudentResponseDto>> GetStudents(int pageNumber, StudentFilterDto searchModel);
    Task<List<StudentResponseDto>> GetDistinctStudents();
    Task<StudentResponseDto> GetStudentById(int StudentId);
    Task<int> InsertStudent(StudentRequestDto insertRequestModel);
    Task<int> UpdateStudent(int StudentId, StudentRequestDto updateRequestModel);
    Task<int> DeleteStudent(int StudentId, StudentRequestDto deleteRequestModel);
}
