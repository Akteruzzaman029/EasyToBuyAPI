using Core.Model;
using Core.ModelDto.BookingCheckList;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class BookingCheckListRepository : IBookingCheckListRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public BookingCheckListRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<BookingCheckListResponseDto>> GetBookingCheckLists(int pageNumber, BookingCheckListFilterDto searchModel)
    {
        PaginatedListModel<BookingCheckListResponseDto> output = new PaginatedListModel<BookingCheckListResponseDto>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.CheckListName);
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<BookingCheckListResponseDto, dynamic>("USP_BookingCheckList_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<BookingCheckListResponseDto>
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

    public async Task<List<BookingCheckListResponseDto>> GetDistinctBookingCheckLists(BookingCheckListFilterDto searchModel)
    {
        var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
        var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();

        p.Add("Name", searchModel.CheckListName);
        p.Add("StartDate", startDate);
        p.Add("EndDate", endDate);
        p.Add("IsActive", searchModel.IsActive);

        var output = await _dataAccessHelper.QueryData<BookingCheckListResponseDto, dynamic>("USP_BookingCheckList_GetDistinct", p);

        return output;
    }

    public async Task<List<BookingCheckListResponseDto>> GetBookingCheckListByBookingId(int BookingId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("BookingId", BookingId);
        var output = await _dataAccessHelper.QueryData<BookingCheckListResponseDto, dynamic>("USP_BookingCheckList_GetByBookingId", p);
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
    
    public async Task<List<dynamic>> GetDayWiseBookingCheckLists(DateTime StartDate)
    {
        var startDate = StartDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_GetDayWiseBookingCheckLists", p);
        return output;
    }
    public async Task<BookingCheckListResponseDto> GetBookingCheckListById(int BookingCheckListId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingCheckListId);
        return (await _dataAccessHelper.QueryData<BookingCheckListResponseDto, dynamic>("USP_BookingCheckList_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertBookingCheckList(BookingCheckListRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("CheckListId", insertRequestModel.CheckListId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_BookingCheckList_Insert", p);
        return p.Get<int>("Id");
    }



    public async Task<int> UpdateBookingCheckList(int BookingCheckListId, BookingCheckListRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingCheckListId);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("CheckListId", insertRequestModel.CheckListId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_BookingCheckList_Update", p);
    }

    public async Task<int> DeleteBookingCheckList(int BookingCheckListId, BookingCheckListRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", BookingCheckListId);

        return await _dataAccessHelper.ExecuteData("USP_BookingCheckList_Delete", p);
    }
}
