using Core.Model;
using Core.ModelDto.OrderItem;

namespace Infrastructure.IRepository;

public interface IOrderItemRepository
{
    Task<PaginatedListModel<OrderItemResponseDto>> GetOrderItems(int pageNumber, OrderItemFilterDto searchModel);
    Task<List<OrderItemResponseDto>> GetDistinctOrderItems(OrderItemFilterDto searchModel);
    Task<OrderItemResponseDto> GetOrderItemById(int OrderItemId);
    Task<List<OrderItemResponseDto>> GetOrderItemsByName(OrderItemRequestDto insertRequestModel);
    Task<int> InsertOrderItem(OrderItemRequestDto insertRequestModel);
    Task<int> UpdateOrderItem(int OrderItemId, OrderItemRequestDto updateRequestModel);
    Task<int> DeleteOrderItem(int OrderItemId, OrderItemRequestDto deleteRequestModel);
}
