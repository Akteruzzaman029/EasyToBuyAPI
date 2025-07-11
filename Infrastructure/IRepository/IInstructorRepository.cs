using Core.Model;
using Core.ModelDto.Instructor;

namespace Infrastructure.IRepository;

public interface IInstructorRepository
{
    Task<PaginatedListModel<InstructorResponseDto>> GetInstructors(int pageNumber, InstructorFilterDto searchModel);
    Task<List<InstructorResponseDto>> GetDistinctInstructors();
    Task<InstructorResponseDto> GetInstructorById(int InstructorId);
    Task<int> InsertInstructor(InstructorRequestDto insertRequestModel);
    Task<int> UpdateInstructor(int InstructorId, InstructorRequestDto updateRequestModel);
    Task<int> DeleteInstructor(int InstructorId, InstructorRequestDto deleteRequestModel);
}
