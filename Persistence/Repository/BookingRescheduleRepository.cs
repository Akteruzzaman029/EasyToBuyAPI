using Core.Model;
using Core.ModelDto.BookingReschedule;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class BookingRescheduleRepository : IBookingRescheduleRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public BookingRescheduleRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<BookingRescheduleResponseDto>> GetBookingReschedules(int pageNumber, BookingRescheduleFilterDto searchModel)
    {
        PaginatedListModel<BookingRescheduleResponseDto> output = new PaginatedListModel<BookingRescheduleResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<BookingRescheduleResponseDto, dynamic>("USP_BookingReschedule_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<BookingRescheduleResponseDto>
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

    public async Task<List<BookingRescheduleResponseDto>> GetDistinctBookingReschedules(int postId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PostId", postId);

        var output = await _dataAccessHelper.QueryData<BookingRescheduleResponseDto, dynamic>("USP_BookingReschedule_GetDistinct", p);

        return output;
    }

    public async Task<BookingRescheduleResponseDto> GetBookingRescheduleById(int BookingRescheduleId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingRescheduleId);
        return (await _dataAccessHelper.QueryData<BookingRescheduleResponseDto, dynamic>("USP_BookingReschedule_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertBookingReschedule(BookingRescheduleRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("NewClassDate", insertRequestModel.NewClassDate);
        p.Add("OldClassDate", insertRequestModel.OldClassDate);
        p.Add("Reason", insertRequestModel.Reason);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_BookingReschedule_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateBookingReschedule(int BookingRescheduleId, BookingRescheduleRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingRescheduleId);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("NewClassDate", insertRequestModel.NewClassDate);
        p.Add("OldClassDate", insertRequestModel.OldClassDate);
        p.Add("Reason", insertRequestModel.Reason);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_BookingReschedule_Update", p);
    }

    public async Task<int> DeleteBookingReschedule(int BookingRescheduleId, BookingRescheduleRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingRescheduleId);

        return await _dataAccessHelper.ExecuteData("USP_BookingReschedule_Delete", p);
    }
}
