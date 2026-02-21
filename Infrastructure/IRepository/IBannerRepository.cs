using Core.Model;
using Core.ModelDto.Banner;

namespace Infrastructure.IRepository;

public interface IBannerRepository
{
    Task<PaginatedListModel<BannerResponseDto>> GetCategories(int pageNumber, BannerFilterDto searchModel);
    Task<List<BannerResponseDto>> GetDistinctCategories(BannerFilterDto searchModel);
    Task<BannerResponseDto> GetBannerById(int BannerId);
    Task<int> InsertBanner(BannerRequestDto insertRequestModel);
    Task<int> UpdateBanner(int BannerId, BannerRequestDto updateRequestModel);
    Task<int> DeleteBanner(int BannerId, BannerRequestDto deleteRequestModel);
}
