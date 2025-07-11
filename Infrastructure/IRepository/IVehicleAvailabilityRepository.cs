using Core.Model;
using Core.ModelDto.VehicleAvailability;

namespace Infrastructure.IRepository;

public interface IVehicleAvailabilityRepository
{
    Task<PaginatedListModel<VehicleAvailabilityResponseDto>> GetVehicleAvailabilitys(int pageNumber, VehicleAvailabilityFilterDto searchModel);
    Task<List<VehicleAvailabilityResponseDto>> GetDistinctVehicleAvailabilitys(VehicleAvailabilityFilterDto searchModel);
    Task<VehicleAvailabilityResponseDto> GetVehicleAvailabilityById(int VehicleAvailabilityId);
    Task<VehicleAvailabilityResponseDto> GetVehicleAvailabilityByName(VehicleAvailabilityRequestDto insertRequestModel);
    Task<int> InsertVehicleAvailability(VehicleAvailabilityRequestDto insertRequestModel);
    Task<int> UpdateVehicleAvailability(int VehicleAvailabilityId, VehicleAvailabilityRequestDto updateRequestModel);
    Task<int> DeleteVehicleAvailability(int VehicleAvailabilityId, VehicleAvailabilityRequestDto deleteRequestModel);
}
