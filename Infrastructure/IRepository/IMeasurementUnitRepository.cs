using Core.Model;
using Core.ModelDto.MeasurementUnit;

namespace Infrastructure.IRepository;

public interface IMeasurementUnitRepository
{
    Task<PaginatedListModel<MeasurementUnitResponseDto>> GetMeasurementUnits(int pageNumber, MeasurementUnitFilterDto searchModel);
    Task<List<MeasurementUnitResponseDto>> GetDistinctMeasurementUnits(MeasurementUnitFilterDto searchModel);
    Task<MeasurementUnitResponseDto> GetMeasurementUnitById(int MeasurementUnitId);
    Task<List<MeasurementUnitResponseDto>> GetMeasurementUnitsByName(MeasurementUnitRequestDto insertRequestModel);
    Task<int> InsertMeasurementUnit(MeasurementUnitRequestDto insertRequestModel);
    Task<int> UpdateMeasurementUnit(int MeasurementUnitId, MeasurementUnitRequestDto updateRequestModel);
    Task<int> DeleteMeasurementUnit(int MeasurementUnitId, MeasurementUnitRequestDto deleteRequestModel);
}
