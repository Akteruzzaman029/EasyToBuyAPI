using Core.Model;
using Core.ModelDto.Order;

namespace Infrastructure.IRepository;

public interface IOrderRepository
{
    Task<PaginatedListModel<OrderResponseDto>> GetOrders(int pageNumber, OrderFilterDto searchModel);
    Task<List<OrderResponseDto>> GetDistinctOrders(OrderFilterDto searchModel);
    Task<OrderResponseDto> GetOrderById(int OrderId);
    Task<List<OrderResponseDto>> GetOrdersByName(OrderRequestDto insertRequestModel);
    Task<int> InsertOrder(OrderRequestDto insertRequestModel);
    Task<int> UpdateOrder(int OrderId, OrderRequestDto updateRequestModel);
    Task<int> DeleteOrder(int OrderId, OrderRequestDto deleteRequestModel);
}
