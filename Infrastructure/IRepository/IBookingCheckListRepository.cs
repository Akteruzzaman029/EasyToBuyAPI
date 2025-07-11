using Core.Model;
using Core.ModelDto.BookingCheckList;

namespace Infrastructure.IRepository;

public interface IBookingCheckListRepository
{
    Task<PaginatedListModel<BookingCheckListResponseDto>> GetBookingCheckLists(int pageNumber, BookingCheckListFilterDto searchModel);
    Task<List<BookingCheckListResponseDto>> GetDistinctBookingCheckLists(BookingCheckListFilterDto searchModel);
    Task<List<BookingCheckListResponseDto>> GetBookingCheckListByBookingId(int BookingId);
    Task<List<dynamic>> GetMonthlySlotAvailability(DateTime startDate);
    Task<List<dynamic>> GetDayWiseBookingCheckLists(DateTime startDate);
    Task<BookingCheckListResponseDto> GetBookingCheckListById(int BookingCheckListId);
    Task<int> InsertBookingCheckList(BookingCheckListRequestDto insertRequestModel);
    Task<int> UpdateBookingCheckList(int BookingCheckListId, BookingCheckListRequestDto updateRequestModel);
    Task<int> DeleteBookingCheckList(int BookingCheckListId, BookingCheckListRequestDto deleteRequestModel);
}
