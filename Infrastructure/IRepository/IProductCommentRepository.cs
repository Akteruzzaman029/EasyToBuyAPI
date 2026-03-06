using Core.Model;
using Core.ModelDto.ProductComment;

namespace Infrastructure.IRepository;

public interface IProductCommentRepository
{
    Task<PaginatedListModel<ProductCommentResponseDto>> GetProductComments(int pageNumber, ProductCommentFilterDto searchModel);
    Task<List<ProductCommentResponseDto>> GetDistinctProductComments(ProductCommentFilterDto searchModel);
    Task<ProductCommentResponseDto> GetProductCommentById(int ProductCommentId);
    Task<List<ProductCommentResponseDto>> GetProductCommentsByName(ProductCommentRequestDto insertRequestModel);
    Task<int> InsertProductComment(ProductCommentRequestDto insertRequestModel);
    Task<int> UpdateProductComment(int ProductCommentId, ProductCommentRequestDto updateRequestModel);
    Task<int> DeleteProductComment(int ProductCommentId, ProductCommentRequestDto deleteRequestModel);
}
