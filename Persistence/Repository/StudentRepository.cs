using Core.Model;
using Core.ModelDto.Student;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using static Core.BaseEnum;

namespace Persistence.Repository;

public class StudentRepository : IStudentRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public StudentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<StudentResponseDto>> GetStudents(int pageNumber, StudentFilterDto searchModel)
    {
        PaginatedListModel<StudentResponseDto> output = new PaginatedListModel<StudentResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("StudentIdNo", searchModel.StudentIdNo);
            p.Add("Name", searchModel.Name);
            p.Add("Email", searchModel.Email);
            p.Add("Phone", searchModel.Phone);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<StudentResponseDto, dynamic>("USP_Student_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<StudentResponseDto>
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

    public async Task<List<StudentResponseDto>> GetDistinctStudents()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<StudentResponseDto, dynamic>("USP_Student_GetDistinct", p);
        return output;
    }

    public async Task<StudentResponseDto> GetStudentById(int StudentId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", StudentId);
        return (await _dataAccessHelper.QueryData<StudentResponseDto, dynamic>("USP_Student_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertStudent(StudentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Email", insertRequestModel.Email);
        p.Add("Phone", insertRequestModel.Phone);
        p.Add("Address", insertRequestModel.Address);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("DateOfBirth", insertRequestModel.DateOfBirth);
        p.Add("LearningStage", insertRequestModel.LearningStage);
        p.Add("VehicleType", insertRequestModel.VehicleType);
        p.Add("PostalCode", insertRequestModel.PostalCode);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Student_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateStudent(int StudentId, StudentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", StudentId);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Email", insertRequestModel.Email);
        p.Add("Phone", insertRequestModel.Phone);
        p.Add("Address", insertRequestModel.Address);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("DateOfBirth", insertRequestModel.DateOfBirth);
        p.Add("LearningStage", insertRequestModel.LearningStage);
        p.Add("VehicleType", insertRequestModel.VehicleType);
        p.Add("PostalCode", insertRequestModel.PostalCode);
        p.Add("BookingId", insertRequestModel.BookingId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Student_Update", p);
    }

    public async Task<int> DeleteStudent(int StudentId, StudentRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", StudentId);
        return await _dataAccessHelper.ExecuteData("USP_Student_Delete", p);
    }
}
