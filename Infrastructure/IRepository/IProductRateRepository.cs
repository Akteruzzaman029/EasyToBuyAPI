using Core.Model;
using Core.ModelDto.ProductRate;

namespace Infrastructure.IRepository;

public interface IProductRateRepository
{
    Task<PaginatedListModel<ProductRateResponseDto>> GetProductRates(int pageNumber, ProductRateFilterDto searchModel);
    Task<List<ProductRateResponseDto>> GetDistinctProductRates(ProductRateFilterDto searchModel);
    Task<ProductRateResponseDto> GetProductRateById(int ProductRateId);
    Task<List<ProductRateResponseDto>> GetProductRatesByName(ProductRateRequestDto insertRequestModel);
    Task<int> InsertProductRate(ProductRateRequestDto insertRequestModel);
    Task<int> UpdateProductRate(int ProductRateId, ProductRateRequestDto updateRequestModel);
    Task<int> DeleteProductRate(int ProductRateId, ProductRateRequestDto deleteRequestModel);
}
