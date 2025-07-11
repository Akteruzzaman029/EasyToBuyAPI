using Core.Model;
using Core.ModelDto.UserPackage;

namespace Infrastructure.IRepository;

public interface IUserPackageRepository
{
    Task<PaginatedListModel<UserPackageResponseDto>> GetUserPackages(int pageNumber, UserPackageFilterDto searchModel);
    Task<List<dynamic>> GetUserPackagesDueList(UserPackageFilterDto searchModel);
    Task<List<dynamic>> GetProfitAndLoss(UserPackageFilterDto filterDto);
    Task<List<dynamic>> GetProfitAndLossDetail(UserPackageFilterDto filterDto);
    Task<List<UserPackageResponseDto>> GetDistinctUserPackages(int postId);
    Task<UserPackageResponseDto> GetUserPackageByStudentId(int studentID);
    Task<UserPackageResponseDto> GetUserPackageById(int UserPackageId);
    Task<int> InsertUserPackage(UserPackageRequestDto insertRequestModel);
    Task<int> UpdateUserPackage(int UserPackageId, UserPackageRequestDto updateRequestModel);
    Task<int> DeleteUserPackage(int UserPackageId, UserPackageRequestDto deleteRequestModel);
}
