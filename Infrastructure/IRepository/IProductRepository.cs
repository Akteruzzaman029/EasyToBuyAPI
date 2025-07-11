using Core.Model;
using Core.ModelDto.Product;

namespace Infrastructure.IRepository;

public interface IProductRepository
{
    Task<PaginatedListModel<ProductResponseDto>> GetProducts(int pageNumber, ProductFilterDto searchModel);
    Task<List<ProductResponseDto>> GetDistinctProducts(ProductFilterDto searchModel);
    Task<ProductResponseDto> GetProductById(int ProductId);
    Task<List<ProductResponseDto>> GetProductsByName(ProductRequestDto insertRequestModel);
    Task<int> InsertProduct(ProductRequestDto insertRequestModel);
    Task<int> UpdateProduct(int ProductId, ProductRequestDto updateRequestModel);
    Task<int> DeleteProduct(int ProductId, ProductRequestDto deleteRequestModel);
}
