using Core.Model;
using Core.ModelDto.Slot;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Persistence.Repository;

public class SlotRepository : ISlotRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public SlotRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<SlotResponseDto>> GetSlots(int pageNumber, SlotFilterDto searchModel)
    {
        PaginatedListModel<SlotResponseDto> output = new PaginatedListModel<SlotResponseDto>();

        try
        {

            var date = searchModel.Date.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("Date", date);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<SlotResponseDto, dynamic>("USP_Slot_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<SlotResponseDto>
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

    public async Task<List<SlotResponseDto>> GetDistinctSlots(DateTime StartDate)
    {
        var date = StartDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("Date", date);
        var output = await _dataAccessHelper.QueryData<SlotResponseDto, dynamic>("USP_Slot_GetDistinct", p);

        return output;
    }

    public async Task<SlotResponseDto> GetSlotById(int SlotId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotId);
        return (await _dataAccessHelper.QueryData<SlotResponseDto, dynamic>("USP_Slot_GetById", p)).FirstOrDefault();
    }
    
    public async Task<SlotResponseDto> GetSlotByName(SlotRequestDto insertRequestModel)
    {
        var date = insertRequestModel.Date.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("Name", insertRequestModel.Name);
        p.Add("Date", date);
        return (await _dataAccessHelper.QueryData<SlotResponseDto, dynamic>("USP_Slot_GetByName", p)).FirstOrDefault();
    }


    public async Task<List<dynamic>> GetMonthlySlot(DateTime StartDate)
    {
        var startDate = StartDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_Slot_GetMonthlySlot", p);
        return output;
    }

    public async Task<int> InsertSlot(SlotRequestDto insertRequestModel)
    {
        var date = insertRequestModel.Date.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Date", date);
        p.Add("StartTime", insertRequestModel.StartTime);
        p.Add("EndTime", insertRequestModel.EndTime);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Slot_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateSlot(int SlotId, SlotRequestDto insertRequestModel)
    {
        var date = insertRequestModel.Date.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Date", date);
        p.Add("StartTime", insertRequestModel.StartTime);
        p.Add("EndTime", insertRequestModel.EndTime);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); 

        return await _dataAccessHelper.ExecuteData("USP_Slot_Update", p);
    }

    public async Task<int> DeleteSlot(int SlotId, SlotRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotId);

        return await _dataAccessHelper.ExecuteData("USP_Slot_Delete", p);
    }
}
