using Core.Model;
using Core.ModelDto.PackType;

namespace Infrastructure.IRepository;

public interface IPackTypeRepository
{
    Task<PaginatedListModel<PackTypeResponseDto>> GetPackTypes(int pageNumber, PackTypeFilterDto searchModel);
    Task<List<PackTypeResponseDto>> GetDistinctPackTypes(PackTypeFilterDto searchModel);
    Task<PackTypeResponseDto> GetPackTypeById(int PackTypeId);
    Task<List<PackTypeResponseDto>> GetPackTypesByName(PackTypeRequestDto insertRequestModel);
    Task<int> InsertPackType(PackTypeRequestDto insertRequestModel);
    Task<int> UpdatePackType(int PackTypeId, PackTypeRequestDto updateRequestModel);
    Task<int> DeletePackType(int PackTypeId, PackTypeRequestDto deleteRequestModel);
}
