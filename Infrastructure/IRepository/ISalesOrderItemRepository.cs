using Core.Model;
using Core.ModelDto.SalesOrderItem;

namespace Infrastructure.IRepository;

public interface ISalesOrderItemRepository
{
    Task<PaginatedListModel<SalesOrderItemResponseDto>> GetSalesOrderItems(int pageNumber, SalesOrderItemFilterDto searchModel);
    Task<List<SalesOrderItemResponseDto>> GetDistinctSalesOrderItems(SalesOrderItemFilterDto searchModel);
    Task<SalesOrderItemResponseDto> GetSalesOrderItemById(int SalesOrderItemId);
    Task<int> InsertSalesOrderItem(SalesOrderItemRequestDto insertRequestModel);
    Task<int> UpdateSalesOrderItem(int SalesOrderItemId, SalesOrderItemRequestDto updateRequestModel);
    Task<int> DeleteSalesOrderItem(int SalesOrderItemId, SalesOrderItemRequestDto deleteRequestModel);
}
