using Core.Model;
using Core.ModelDto.LessonProgres;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class LessonProgresRepository : ILessonProgresRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public LessonProgresRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<LessonProgresResponseDto>> GetLessonProgress(int pageNumber, LessonProgresFilterDto searchModel)
    {
        PaginatedListModel<LessonProgresResponseDto> output = new PaginatedListModel<LessonProgresResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<LessonProgresResponseDto, dynamic>("USP_LessonProgres_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<LessonProgresResponseDto>
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

    public async Task<List<LessonProgresResponseDto>> GetDistinctLessonProgress(int BookingId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("BookingId", BookingId);

        var output = await _dataAccessHelper.QueryData<LessonProgresResponseDto, dynamic>("USP_LessonProgres_GetDistinct", p);

        return output;
    }

    public async Task<LessonProgresResponseDto> GetLessonProgresById(int LessonProgresId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", LessonProgresId);
        return (await _dataAccessHelper.QueryData<LessonProgresResponseDto, dynamic>("USP_LessonProgres_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertLessonProgres(LessonProgresRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Status", insertRequestModel.Status);
        p.Add("LessonTitle", insertRequestModel.LessonTitle);
        p.Add("Feedback", insertRequestModel.Feedback);
        p.Add("ProgressPercentage", insertRequestModel.ProgressPercentage);
        p.Add("AddedBy", insertRequestModel.AddedBy);
        p.Add("AddedDate", insertRequestModel.AddedDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_LessonProgres_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> LessonProgres(LessonProgresDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Status", insertRequestModel.Status);
        p.Add("NewClassDate", insertRequestModel.NewClassDate);
        p.Add("NewSlotId", insertRequestModel.NewSlotId);
        p.Add("OldClassDate", insertRequestModel.OldClassDate);
        p.Add("OldSlotId", insertRequestModel.OldSlotId);
        p.Add("Reason", insertRequestModel.Reason);
        p.Add("LessonTitle", insertRequestModel.LessonTitle);
        p.Add("Feedback", insertRequestModel.Feedback);
        p.Add("ProgressPercentage", insertRequestModel.ProgressPercentage);
        p.Add("AddedBy", insertRequestModel.AddedBy);
        p.Add("AddedDate", insertRequestModel.AddedDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_LessonProgres", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateLessonProgres(int LessonProgresId, LessonProgresRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", LessonProgresId);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Status", insertRequestModel.Status);
        p.Add("LessonTitle", insertRequestModel.LessonTitle);
        p.Add("Feedback", insertRequestModel.Feedback);
        p.Add("ProgressPercentage", insertRequestModel.ProgressPercentage);
        p.Add("AddedBy", insertRequestModel.AddedBy);
        p.Add("AddedDate", insertRequestModel.AddedDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_LessonProgres_Update", p);
    }

    public async Task<int> DeleteLessonProgres(int LessonProgresId, LessonProgresRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", LessonProgresId);

        return await _dataAccessHelper.ExecuteData("USP_LessonProgres_Delete", p);
    }
}
