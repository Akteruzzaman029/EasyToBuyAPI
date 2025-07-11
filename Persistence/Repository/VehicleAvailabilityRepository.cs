using Core.Model;
using Core.ModelDto.VehicleAvailability;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class VehicleAvailabilityRepository : IVehicleAvailabilityRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public VehicleAvailabilityRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<VehicleAvailabilityResponseDto>> GetVehicleAvailabilitys(int pageNumber, VehicleAvailabilityFilterDto searchModel)
    {
        PaginatedListModel<VehicleAvailabilityResponseDto> output = new PaginatedListModel<VehicleAvailabilityResponseDto>();

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

            var result = await _dataAccessHelper.QueryData<VehicleAvailabilityResponseDto, dynamic>("USP_VehicleAvailability_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<VehicleAvailabilityResponseDto>
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

    public async Task<List<VehicleAvailabilityResponseDto>> GetDistinctVehicleAvailabilitys(VehicleAvailabilityFilterDto searchModel)
    {
        DynamicParameters p = new DynamicParameters();
        var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
        p.Add("AvailableDate", startDate);

        var output = await _dataAccessHelper.QueryData<VehicleAvailabilityResponseDto, dynamic>("USP_VehicleAvailability_GetDistinct", p);

        return output;
    }

    public async Task<VehicleAvailabilityResponseDto> GetVehicleAvailabilityById(int VehicleAvailabilityId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleAvailabilityId);
        return (await _dataAccessHelper.QueryData<VehicleAvailabilityResponseDto, dynamic>("USP_VehicleAvailability_GetById", p)).FirstOrDefault();
    }


    public async Task<VehicleAvailabilityResponseDto> GetVehicleAvailabilityByName(VehicleAvailabilityRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("VehicleId", insertRequestModel.VehicleId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        return (await _dataAccessHelper.QueryData<VehicleAvailabilityResponseDto, dynamic>("USP_VehicleAvailability_GetByName", p)).FirstOrDefault();
    }
    
    public async Task<int> InsertVehicleAvailability(VehicleAvailabilityRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("VehicleId", insertRequestModel.VehicleId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_VehicleAvailability_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateVehicleAvailability(int VehicleAvailabilityId, VehicleAvailabilityRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleAvailabilityId);
        p.Add("SlotId", insertRequestModel.SlotId);
        p.Add("VehicleId", insertRequestModel.VehicleId);
        p.Add("AvailableDate", insertRequestModel.AvailableDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_VehicleAvailability_Update", p);
    }

    public async Task<int> DeleteVehicleAvailability(int VehicleAvailabilityId, VehicleAvailabilityRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleAvailabilityId);

        return await _dataAccessHelper.ExecuteData("USP_VehicleAvailability_Delete", p);
    }
}
