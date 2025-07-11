using Core.Model;
using Core.ModelDto.LessonProgres;

namespace Infrastructure.IRepository;

public interface ILessonProgresRepository
{
    Task<PaginatedListModel<LessonProgresResponseDto>> GetLessonProgress(int pageNumber, LessonProgresFilterDto searchModel);
    Task<List<LessonProgresResponseDto>> GetDistinctLessonProgress(int postId);
    Task<LessonProgresResponseDto> GetLessonProgresById(int LessonProgresId);
    Task<int> InsertLessonProgres(LessonProgresRequestDto insertRequestModel);
    Task<int> LessonProgres(LessonProgresDto insertRequestModel);
    Task<int> UpdateLessonProgres(int LessonProgresId, LessonProgresRequestDto updateRequestModel);
    Task<int> DeleteLessonProgres(int LessonProgresId, LessonProgresRequestDto deleteRequestModel);
}
