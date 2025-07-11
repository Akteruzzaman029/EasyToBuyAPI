using Core.Model;
using Core.ModelDto.ExpenditureHead;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class ExpenditureHeadRepository : IExpenditureHeadRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public ExpenditureHeadRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<ExpenditureHeadResponseDto>> GetExpenditureHeads(int pageNumber, ExpenditureHeadFilterDto searchModel)
    {
        PaginatedListModel<ExpenditureHeadResponseDto> output = new PaginatedListModel<ExpenditureHeadResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<ExpenditureHeadResponseDto, dynamic>("USP_ExpenditureHead_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<ExpenditureHeadResponseDto>
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

    public async Task<List<ExpenditureHeadResponseDto>> GetDistinctExpenditureHeads()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<ExpenditureHeadResponseDto, dynamic>("USP_ExpenditureHead_GetDistinct", p);

        return output;
    }

    public async Task<ExpenditureHeadResponseDto> GetExpenditureHeadById(int ExpenditureHeadId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureHeadId);
        return (await _dataAccessHelper.QueryData<ExpenditureHeadResponseDto, dynamic>("USP_ExpenditureHead_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertExpenditureHead(ExpenditureHeadRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_ExpenditureHead_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateExpenditureHead(int ExpenditureHeadId, ExpenditureHeadRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureHeadId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_ExpenditureHead_Update", p);
    }

    public async Task<int> DeleteExpenditureHead(int ExpenditureHeadId, ExpenditureHeadRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", ExpenditureHeadId);
        return await _dataAccessHelper.ExecuteData("USP_ExpenditureHead_Delete", p);
    }
}
