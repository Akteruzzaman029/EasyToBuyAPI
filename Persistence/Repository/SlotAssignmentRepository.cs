using Core.Model;
using Core.ModelDto.SlotAssignment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class SlotAssignmentRepository : ISlotAssignmentRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public SlotAssignmentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<SlotAssignmentResponseDto>> GetSlotAssignments(int pageNumber, SlotAssignmentFilterDto searchModel)
    {
        PaginatedListModel<SlotAssignmentResponseDto> output = new PaginatedListModel<SlotAssignmentResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<SlotAssignmentResponseDto, dynamic>("USP_SlotAssignment_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<SlotAssignmentResponseDto>
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

    public async Task<List<SlotAssignmentResponseDto>> GetDistinctSlotAssignments(int postId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PostId", postId);

        var output = await _dataAccessHelper.QueryData<SlotAssignmentResponseDto, dynamic>("USP_SlotAssignment_GetDistinct", p);

        return output;
    }

    public async Task<SlotAssignmentResponseDto> GetSlotAssignmentById(int SlotAssignmentId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotAssignmentId);
        return (await _dataAccessHelper.QueryData<SlotAssignmentResponseDto, dynamic>("USP_SlotAssignment_GetById", p)).FirstOrDefault();
    }


    public async Task<SlotAssignmentResponseDto> GetSlotAssignmentByName(SlotAssignmentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("InstructorId", insertRequestModel.InstructorId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        return (await _dataAccessHelper.QueryData<SlotAssignmentResponseDto, dynamic>("USP_SlotAssignment_GetByName", p)).FirstOrDefault();
    }
    
    public async Task<int> InsertSlotAssignment(SlotAssignmentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("InstructorId", insertRequestModel.InstructorId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_SlotAssignment_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateSlotAssignment(int SlotAssignmentId, SlotAssignmentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotAssignmentId);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("InstructorId", insertRequestModel.InstructorId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_SlotAssignment_Update", p);
    }

    public async Task<int> DeleteSlotAssignment(int SlotAssignmentId, SlotAssignmentRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", SlotAssignmentId);

        return await _dataAccessHelper.ExecuteData("USP_SlotAssignment_Delete", p);
    }
}
