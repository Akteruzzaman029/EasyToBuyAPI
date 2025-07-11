using Core.Model;
using Core.ModelDto.Booking;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public BookingRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<BookingResponseDto>> GetBookings(int pageNumber, BookingFilterDto searchModel)
    {
        PaginatedListModel<BookingResponseDto> output = new PaginatedListModel<BookingResponseDto>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<BookingResponseDto, dynamic>("USP_Booking_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<BookingResponseDto>
            {
                PageIndex = pageNumber,
                TotalRecords = TotalRecords,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages,
                Items = result.ToList()
            };

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return output;
    }

    public async Task<List<BookingResponseDto>> GetDistinctBookings(BookingFilterDto searchModel)
    {
        var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
        var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();

        p.Add("Name", searchModel.Name);
        p.Add("StartDate", startDate);
        p.Add("EndDate", endDate);
        p.Add("IsActive", searchModel.IsActive);

        var output = await _dataAccessHelper.QueryData<BookingResponseDto, dynamic>("USP_Booking_GetDistinct", p);

        return output;
    }

    public async Task<List<BookingResponseDto>> GetBookingByStudentId(int StudentID)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("StudentID", StudentID);
        var output = await _dataAccessHelper.QueryData<BookingResponseDto, dynamic>("USP_Booking_GetByStudentId", p);
        return output;
    }   
    
    public async Task<List<dynamic>> GetMonthlySlotAvailability(DateTime StartDate)
    {
        var startDate = StartDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_GetMonthlySlotAvailability", p);
        return output;
    }  
    
    public async Task<List<dynamic>> GetDayWiseBookings(DateTime StartDate)
    {
        var startDate = StartDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_GetDayWiseBookings", p);
        return output;
    }
    public async Task<BookingResponseDto> GetBookingById(int BookingId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingId);
        return (await _dataAccessHelper.QueryData<BookingResponseDto, dynamic>("USP_Booking_GetById", p)).FirstOrDefault();
    }

    public async Task<BookingResponseDto> GetBookingByName(BookingRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("ClassDate", insertRequestModel.ClassDate);
        p.Add("slotId", insertRequestModel.slotId);
        return (await _dataAccessHelper.QueryData<BookingResponseDto, dynamic>("USP_Booking_GetByName", p)).FirstOrDefault();
    }


    public async Task<int> InsertBooking(BookingRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("StudentId", insertRequestModel.StudentId);
        p.Add("slotId", insertRequestModel.slotId);
        p.Add("ClassDate", insertRequestModel.ClassDate);
        p.Add("Status", insertRequestModel.Status);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Booking_Insert", p);
        return p.Get<int>("Id");
    }



    public async Task<int> UpdateBooking(int BookingId, BookingRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingId);
        p.Add("StudentId", insertRequestModel.StudentId);
        p.Add("slotId", insertRequestModel.slotId);
        p.Add("ClassDate", insertRequestModel.ClassDate);
        p.Add("Status", insertRequestModel.Status);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        p.Add("IsRepeat", insertRequestModel.IsRepeat);

        return await _dataAccessHelper.ExecuteData("USP_Booking_Update", p);
    }

    public async Task<int> DeleteBooking(int BookingId, BookingRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingId);

        return await _dataAccessHelper.ExecuteData("USP_Booking_Delete", p);
    }
}
