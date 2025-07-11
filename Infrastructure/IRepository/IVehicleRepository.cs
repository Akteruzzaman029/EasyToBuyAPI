using Core.Model;
using Core.ModelDto.Vehicle;

namespace Infrastructure.IRepository;

public interface IVehicleRepository
{
    Task<PaginatedListModel<VehicleResponseDto>> GetVehicles(int pageNumber, VehicleFilterDto searchModel);
    Task<List<VehicleResponseDto>> GetDistinctVehicles();
    Task<VehicleResponseDto> GetVehicleById(int VehicleId);
    Task<int> InsertVehicle(VehicleRequestDto insertRequestModel);
    Task<int> UpdateVehicle(int VehicleId, VehicleRequestDto updateRequestModel);
    Task<int> DeleteVehicle(int VehicleId, VehicleRequestDto deleteRequestModel);
}
