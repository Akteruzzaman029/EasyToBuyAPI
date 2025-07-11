using Core.Model;
using Core.ModelDto.Appointment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public AppointmentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<AppointmentResponseDto>> GetAppointments(int pageNumber, AppointmentFilterDto searchModel)
    {
        PaginatedListModel<AppointmentResponseDto> output = new PaginatedListModel<AppointmentResponseDto>();

        try
        {

            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");

            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("SlotName", searchModel.SlotName);
            p.Add("InstructorName", searchModel.InstructorName);
            p.Add("Status", searchModel.Status);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<AppointmentResponseDto, dynamic>("USP_Appointment_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<AppointmentResponseDto>
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

    public async Task<List<AppointmentResponseDto>> GetDistinctAppointments(int instractorId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("InstructorId", instractorId);
        var output = await _dataAccessHelper.QueryData<AppointmentResponseDto, dynamic>("USP_Appointment_GetDistinct", p);

        return output;
    }

    public async Task<AppointmentResponseDto> GetAppointmentById(int AppointmentId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AppointmentId);
        return (await _dataAccessHelper.QueryData<AppointmentResponseDto, dynamic>("USP_Appointment_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertAppointment(AppointmentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("InstructorId", insertRequestModel.InstructorId);
        p.Add("Status", insertRequestModel.Status);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Appointment_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateAppointment(int AppointmentId, AppointmentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AppointmentId);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("InstructorId", insertRequestModel.InstructorId);
        p.Add("Status", insertRequestModel.Status);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Appointment_Update", p);
    }

    public async Task<int> DeleteAppointment(int AppointmentId, AppointmentRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", AppointmentId);

        return await _dataAccessHelper.ExecuteData("USP_Appointment_Delete", p);
    }
}
