using Core.Model;
using Core.ModelDto.OrderDelivery;

namespace Infrastructure.IRepository;

public interface IOrderDeliveryRepository
{
    Task<PaginatedListModel<OrderDeliveryResponseDto>> GetOrderDeliverys(int pageNumber, OrderDeliveryFilterDto searchModel);
    Task<List<OrderDeliveryResponseDto>> GetDistinctOrderDeliverys(OrderDeliveryFilterDto searchModel);
    Task<OrderDeliveryResponseDto> GetOrderDeliveryById(int OrderDeliveryId);
    Task<int> InsertOrderDelivery(OrderDeliveryRequestDto insertRequestModel);
    Task<int> UpdateOrderDelivery(int OrderDeliveryId, OrderDeliveryRequestDto updateRequestModel);
    Task<int> DeleteOrderDelivery(int OrderDeliveryId, OrderDeliveryRequestDto deleteRequestModel);
}
