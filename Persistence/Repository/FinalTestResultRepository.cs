using Core.Model;
using Core.ModelDto.FinalTestResult;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class FinalTestResultRepository : IFinalTestResultRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public FinalTestResultRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<FinalTestResultResponseDto>> GetFinalTestResults(int pageNumber, FinalTestResultFilterDto searchModel)
    {
        PaginatedListModel<FinalTestResultResponseDto> output = new PaginatedListModel<FinalTestResultResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<FinalTestResultResponseDto, dynamic>("USP_FinalTestResult_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<FinalTestResultResponseDto>
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

    public async Task<List<FinalTestResultResponseDto>> GetDistinctFinalTestResults(int postId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PostId", postId);

        var output = await _dataAccessHelper.QueryData<FinalTestResultResponseDto, dynamic>("USP_FinalTestResult_GetDistinct", p);

        return output;
    }

    public async Task<FinalTestResultResponseDto> GetFinalTestResultById(int FinalTestResultId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", FinalTestResultId);
        return (await _dataAccessHelper.QueryData<FinalTestResultResponseDto, dynamic>("USP_FinalTestResult_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertFinalTestResult(FinalTestResultRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("StudentId", insertRequestModel.StudentId);
        p.Add("Instruction", insertRequestModel.Instruction);
        p.Add("TestDate", insertRequestModel.TestDate);
        p.Add("TestType", insertRequestModel.TestType);
        p.Add("Score", insertRequestModel.Score);
        p.Add("Passed", insertRequestModel.Passed);
        p.Add("EvaluatedBy", insertRequestModel.EvaluatedBy);
        p.Add("EvaluatedDate", insertRequestModel.EvaluatedDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_FinalTestResult_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateFinalTestResult(int FinalTestResultId, FinalTestResultRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", FinalTestResultId);
        p.Add("StudentId", insertRequestModel.StudentId);
        p.Add("Instruction", insertRequestModel.Instruction);
        p.Add("TestDate", insertRequestModel.TestDate);
        p.Add("TestType", insertRequestModel.TestType);
        p.Add("Score", insertRequestModel.Score);
        p.Add("Passed", insertRequestModel.Passed);
        p.Add("EvaluatedBy", insertRequestModel.EvaluatedBy);
        p.Add("EvaluatedDate", insertRequestModel.EvaluatedDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_FinalTestResult_Update", p);
    }

    public async Task<int> DeleteFinalTestResult(int FinalTestResultId, FinalTestResultRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", FinalTestResultId);

        return await _dataAccessHelper.ExecuteData("USP_FinalTestResult_Delete", p);
    }
}
