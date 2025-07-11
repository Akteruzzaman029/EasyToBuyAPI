using Core.Model;
using Core.ModelDto.Instructor;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class InstructorRepository : IInstructorRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public InstructorRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<InstructorResponseDto>> GetInstructors(int pageNumber, InstructorFilterDto searchModel)
    {
        PaginatedListModel<InstructorResponseDto> output = new PaginatedListModel<InstructorResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("Email", searchModel.Email);
            p.Add("Phone", searchModel.Phone);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<InstructorResponseDto, dynamic>("USP_Instructor_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<InstructorResponseDto>
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

    public async Task<List<InstructorResponseDto>> GetDistinctInstructors()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<InstructorResponseDto, dynamic>("USP_Instructor_GetDistinct", p);
        return output;
    }

    public async Task<InstructorResponseDto> GetInstructorById(int InstructorId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", InstructorId);
        return (await _dataAccessHelper.QueryData<InstructorResponseDto, dynamic>("USP_Instructor_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertInstructor(InstructorRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Email", insertRequestModel.Email);
        p.Add("Phone", insertRequestModel.Phone);
        p.Add("Address", insertRequestModel.Address);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("LicenseNumber", insertRequestModel.LicenseNumber);
        p.Add("YearsOfExperience", insertRequestModel.YearsOfExperience);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Instructor_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateInstructor(int InstructorId, InstructorRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", InstructorId);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Email", insertRequestModel.Email);
        p.Add("Phone", insertRequestModel.Phone);
        p.Add("Address", insertRequestModel.Address);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("LicenseNumber", insertRequestModel.LicenseNumber);
        p.Add("YearsOfExperience", insertRequestModel.YearsOfExperience);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Instructor_Update", p);
    }

    public async Task<int> DeleteInstructor(int InstructorId, InstructorRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", InstructorId);
        return await _dataAccessHelper.ExecuteData("USP_Instructor_Delete", p);
    }
}
