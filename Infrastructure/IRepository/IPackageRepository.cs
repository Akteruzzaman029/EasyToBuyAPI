using Core.Model;
using Core.ModelDto.Package;

namespace Infrastructure.IRepository;

public interface IPackageRepository
{
    Task<PaginatedListModel<PackageResponseDto>> GetPackages(int pageNumber, PackageFilterDto searchModel);
    Task<List<PackageResponseDto>> GetDistinctPackages();
    Task<PackageResponseDto> GetPackageById(int PackageId);
    Task<int> InsertPackage(PackageRequestDto insertRequestModel);
    Task<int> UpdatePackage(int PackageId, PackageRequestDto updateRequestModel);
    Task<int> DeletePackage(int PackageId, PackageRequestDto deleteRequestModel);
}
