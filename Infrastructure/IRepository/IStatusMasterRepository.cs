using Core.Model;
using Core.ModelDto.StatusMaster;

namespace Infrastructure.IRepository;

public interface IStatusMasterRepository
{
    Task<PaginatedListModel<StatusMasterResponseDto>> GetStatusMasters(int pageNumber, StatusMasterFilterDto searchModel);
    Task<List<StatusMasterResponseDto>> GetDistinctStatusMasters(StatusMasterFilterDto searchModel);
    Task<StatusMasterResponseDto> GetStatusMasterById(int StatusMasterId);
    Task<List<StatusMasterResponseDto>> GetStatusMastersByName(StatusMasterRequestDto insertRequestModel);
    Task<int> InsertStatusMaster(StatusMasterRequestDto insertRequestModel);
    Task<int> UpdateStatusMaster(int StatusMasterId, StatusMasterRequestDto updateRequestModel);
    Task<int> DeleteStatusMaster(int StatusMasterId, StatusMasterRequestDto deleteRequestModel);
}
