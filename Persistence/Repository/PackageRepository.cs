using Core.Model;
using Core.ModelDto.Package;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class PackageRepository : IPackageRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public PackageRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<PackageResponseDto>> GetPackages(int pageNumber, PackageFilterDto searchModel)
    {
        PaginatedListModel<PackageResponseDto> output = new PaginatedListModel<PackageResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<PackageResponseDto, dynamic>("USP_Package_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<PackageResponseDto>
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

    public async Task<List<PackageResponseDto>> GetDistinctPackages()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<PackageResponseDto, dynamic>("USP_Package_GetDistinct", p);

        return output;
    }

    public async Task<PackageResponseDto> GetPackageById(int PackageId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PackageId);
        return (await _dataAccessHelper.QueryData<PackageResponseDto, dynamic>("USP_Package_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertPackage(PackageRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Description", insertRequestModel.Description);
        p.Add("Price", insertRequestModel.Price);
        p.Add("TotalLessons", insertRequestModel.TotalLessons);
        p.Add("Rate", insertRequestModel.Rate);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Package_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdatePackage(int PackageId, PackageRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PackageId);
        p.Add("Name", insertRequestModel.Name);
        p.Add("Description", insertRequestModel.Description);
        p.Add("Price", insertRequestModel.Price);
        p.Add("TotalLessons", insertRequestModel.TotalLessons);
        p.Add("Rate", insertRequestModel.Rate);
        p.Add("FileId", insertRequestModel.FileId);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Package_Update", p);
    }

    public async Task<int> DeletePackage(int PackageId, PackageRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PackageId);

        return await _dataAccessHelper.ExecuteData("USP_Package_Delete", p);
    }
}
