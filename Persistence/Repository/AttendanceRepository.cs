using Core.Model;
using Core.ModelDto.Attendance;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public AttendanceRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<AttendanceResponseDto>> GetAttendances(int pageNumber, AttendanceFilterDto searchModel)
    {
        PaginatedListModel<AttendanceResponseDto> output = new PaginatedListModel<AttendanceResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<AttendanceResponseDto, dynamic>("USP_Attendance_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<AttendanceResponseDto>
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

    public async Task<List<AttendanceResponseDto>> GetDistinctAttendances(int postId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PostId", postId);

        var output = await _dataAccessHelper.QueryData<AttendanceResponseDto, dynamic>("USP_Attendance_GetDistinct", p);

        return output;
    }

    public async Task<AttendanceResponseDto> GetAttendanceById(int AttendanceId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AttendanceId);
        return (await _dataAccessHelper.QueryData<AttendanceResponseDto, dynamic>("USP_Attendance_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertAttendance(AttendanceRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Attended", insertRequestModel.Attended);
        p.Add("MarkBy", insertRequestModel.MarkBy);
        p.Add("MarkDate", insertRequestModel.MarkDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Attendance_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateAttendance(int AttendanceId, AttendanceRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AttendanceId);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Attended", insertRequestModel.Attended);
        p.Add("MarkBy", insertRequestModel.MarkBy);
        p.Add("MarkDate", insertRequestModel.MarkDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Attendance_Update", p);
    }

    public async Task<int> DeleteAttendance(int AttendanceId, AttendanceRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AttendanceId);

        return await _dataAccessHelper.ExecuteData("USP_Attendance_Delete", p);
    }
}
