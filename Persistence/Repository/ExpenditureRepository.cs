using Core.Model;
using Core.ModelDto.Expenditure;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class ExpenditureRepository : IExpenditureRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public ExpenditureRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<ExpenditureResponseDto>> GetExpenditures(int pageNumber, ExpenditureFilterDto searchModel)
    {
        PaginatedListModel<ExpenditureResponseDto> output = new PaginatedListModel<ExpenditureResponseDto>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("ExpenditureHeadId", searchModel.ExpenditureHeadId);
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<ExpenditureResponseDto, dynamic>("USP_Expenditure_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<ExpenditureResponseDto>
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

    public async Task<List<ExpenditureResponseDto>> GetDistinctExpenditures()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<ExpenditureResponseDto, dynamic>("USP_Expenditure_GetDistinct", p);

        return output;
    }

    public async Task<ExpenditureResponseDto> GetExpenditureById(int ExpenditureId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureId);
        return (await _dataAccessHelper.QueryData<ExpenditureResponseDto, dynamic>("USP_Expenditure_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertExpenditure(ExpenditureRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Name", insertRequestModel.Name);
        p.Add("ExpenditureHeadId", insertRequestModel.ExpenditureHeadId);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Expenditure_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateExpenditure(int ExpenditureId, ExpenditureRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("ExpenditureHeadId", insertRequestModel.ExpenditureHeadId);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Expenditure_Update", p);
    }

    public async Task<int> DeleteExpenditure(int ExpenditureId, ExpenditureRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureId);

        return await _dataAccessHelper.ExecuteData("USP_Expenditure_Delete", p);
    }
}
