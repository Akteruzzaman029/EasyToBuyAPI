using Core.Model;
using Core.ModelDto.Appointment;

namespace Infrastructure.IRepository;

public interface IAppointmentRepository
{
    Task<PaginatedListModel<AppointmentResponseDto>> GetAppointments(int pageNumber, AppointmentFilterDto searchModel);
    Task<List<AppointmentResponseDto>> GetDistinctAppointments(int postId);
    Task<AppointmentResponseDto> GetAppointmentById(int AppointmentId);
    Task<int> InsertAppointment(AppointmentRequestDto insertRequestModel);
    Task<int> UpdateAppointment(int AppointmentId, AppointmentRequestDto updateRequestModel);
    Task<int> DeleteAppointment(int AppointmentId, AppointmentRequestDto deleteRequestModel);
}
