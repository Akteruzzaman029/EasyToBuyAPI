using Core.Model;
using Core.ModelDto.CheckList;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class CheckListRepository : ICheckListRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public CheckListRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<CheckListResponseDto>> GetCheckLists(int pageNumber, CheckListFilterDto searchModel)
    {
        PaginatedListModel<CheckListResponseDto> output = new PaginatedListModel<CheckListResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<CheckListResponseDto, dynamic>("USP_CheckList_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<CheckListResponseDto>
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

    public async Task<List<CheckListResponseDto>> GetDistinctCheckLists()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<CheckListResponseDto, dynamic>("USP_CheckList_GetDistinct", p);

        return output;
    }

    public async Task<CheckListResponseDto> GetCheckListById(int CheckListId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", CheckListId);
        return (await _dataAccessHelper.QueryData<CheckListResponseDto, dynamic>("USP_CheckList_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertCheckList(CheckListRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Description", insertRequestModel.Description);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Weight", insertRequestModel.Weight);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_CheckList_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateCheckList(int CheckListId, CheckListRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", CheckListId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Description", insertRequestModel.Description);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Weight", insertRequestModel.Weight);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_CheckList_Update", p);
    }

    public async Task<int> DeleteCheckList(int CheckListId, CheckListRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", CheckListId);
        return await _dataAccessHelper.ExecuteData("USP_CheckList_Delete", p);
    }
}
