using Core.Model;
using Core.ModelDto.Vehicle;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class VehicleRepository : IVehicleRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public VehicleRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<VehicleResponseDto>> GetVehicles(int pageNumber, VehicleFilterDto searchModel)
    {
        PaginatedListModel<VehicleResponseDto> output = new PaginatedListModel<VehicleResponseDto>();

        try
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Model", searchModel.Model);
            p.Add("RegistrationNumber", searchModel.RegistrationNumber);
            p.Add("FuelType", searchModel.FuelType);
            p.Add("TransmissionType", searchModel.TransmissionType);
            p.Add("Status", searchModel.Status);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<VehicleResponseDto, dynamic>("USP_Vehicle_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<VehicleResponseDto>
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

    public async Task<List<VehicleResponseDto>> GetDistinctVehicles()
    {
        DynamicParameters p = new DynamicParameters();
        var output = await _dataAccessHelper.QueryData<VehicleResponseDto, dynamic>("USP_Vehicle_GetDistinct", p);
        return output;
    }

    public async Task<VehicleResponseDto> GetVehicleById(int VehicleId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleId);
        return (await _dataAccessHelper.QueryData<VehicleResponseDto, dynamic>("USP_Vehicle_GetById", p)).FirstOrDefault();
    }


    public async Task<int> InsertVehicle(VehicleRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("Make", insertRequestModel.Make);
        p.Add("Model", insertRequestModel.Model);
        p.Add("Year", insertRequestModel.Year);
        p.Add("RegistrationNumber", insertRequestModel.RegistrationNumber);
        p.Add("Color", insertRequestModel.Color);
        p.Add("FuelType", insertRequestModel.FuelType);
        p.Add("TransmissionType", insertRequestModel.TransmissionType);
        p.Add("Status", insertRequestModel.Status);
        p.Add("LastServicedAt", insertRequestModel.LastServicedAt);
        p.Add("InsuranceExpiryDate", insertRequestModel.InsuranceExpiryDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Vehicle_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateVehicle(int VehicleId, VehicleRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleId);
        p.Add("Make", insertRequestModel.Make);
        p.Add("Model", insertRequestModel.Model);
        p.Add("Year", insertRequestModel.Year);
        p.Add("RegistrationNumber", insertRequestModel.RegistrationNumber);
        p.Add("Color", insertRequestModel.Color);
        p.Add("FuelType", insertRequestModel.FuelType);
        p.Add("TransmissionType", insertRequestModel.TransmissionType);
        p.Add("Status", insertRequestModel.Status);
        p.Add("LastServicedAt", insertRequestModel.LastServicedAt);
        p.Add("InsuranceExpiryDate", insertRequestModel.InsuranceExpiryDate);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Vehicle_Update", p);
    }

    public async Task<int> DeleteVehicle(int VehicleId, VehicleRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", VehicleId);
        return await _dataAccessHelper.ExecuteData("USP_Vehicle_Delete", p);
    }
}
