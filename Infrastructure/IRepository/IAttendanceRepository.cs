using Core.Model;
using Core.ModelDto.Attendance;

namespace Infrastructure.IRepository;

public interface IAttendanceRepository
{
    Task<PaginatedListModel<AttendanceResponseDto>> GetAttendances(int pageNumber, AttendanceFilterDto searchModel);
    Task<List<AttendanceResponseDto>> GetDistinctAttendances(int postId);
    Task<AttendanceResponseDto> GetAttendanceById(int AttendanceId);
    Task<int> InsertAttendance(AttendanceRequestDto insertRequestModel);
    Task<int> UpdateAttendance(int AttendanceId, AttendanceRequestDto updateRequestModel);
    Task<int> DeleteAttendance(int AttendanceId, AttendanceRequestDto deleteRequestModel);
}
