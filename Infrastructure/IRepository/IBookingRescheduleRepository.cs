using Core.Model;
using Core.ModelDto.BookingReschedule;

namespace Infrastructure.IRepository;

public interface IBookingRescheduleRepository
{
    Task<PaginatedListModel<BookingRescheduleResponseDto>> GetBookingReschedules(int pageNumber, BookingRescheduleFilterDto searchModel);
    Task<List<BookingRescheduleResponseDto>> GetDistinctBookingReschedules(int postId);
    Task<BookingRescheduleResponseDto> GetBookingRescheduleById(int BookingRescheduleId);
    Task<int> InsertBookingReschedule(BookingRescheduleRequestDto insertRequestModel);
    Task<int> UpdateBookingReschedule(int BookingRescheduleId, BookingRescheduleRequestDto updateRequestModel);
    Task<int> DeleteBookingReschedule(int BookingRescheduleId, BookingRescheduleRequestDto deleteRequestModel);
}
