using Core.Model;
using Core.ModelDto.Booking;

namespace Infrastructure.IRepository;

public interface IBookingRepository
{
    Task<PaginatedListModel<BookingResponseDto>> GetBookings(int pageNumber, BookingFilterDto searchModel);
    Task<List<BookingResponseDto>> GetDistinctBookings(BookingFilterDto searchModel);
    Task<List<BookingResponseDto>> GetBookingByStudentId(int StudentID);
    Task<List<dynamic>> GetMonthlySlotAvailability(DateTime startDate);
    Task<List<dynamic>> GetDayWiseBookings(DateTime startDate);
    Task<BookingResponseDto> GetBookingById(int BookingId);
    Task<BookingResponseDto> GetBookingByName(BookingRequestDto insertRequestModel);
    Task<int> InsertBooking(BookingRequestDto insertRequestModel);
    Task<int> UpdateBooking(int BookingId, BookingRequestDto updateRequestModel);
    Task<int> DeleteBooking(int BookingId, BookingRequestDto deleteRequestModel);
}
