using Core.Model;
using Core.ModelDto.Brand;

namespace Infrastructure.IRepository;

public interface IBrandRepository
{
    Task<PaginatedListModel<BrandResponseDto>> GetBrands(int pageNumber, BrandFilterDto searchModel);
    Task<List<BrandResponseDto>> GetDistinctBrands(BrandFilterDto searchModel);
    Task<BrandResponseDto> GetBrandById(int BrandId);
    Task<int> InsertBrand(BrandRequestDto insertRequestModel);
    Task<int> UpdateBrand(int BrandId, BrandRequestDto updateRequestModel);
    Task<int> DeleteBrand(int BrandId, BrandRequestDto deleteRequestModel);
}
