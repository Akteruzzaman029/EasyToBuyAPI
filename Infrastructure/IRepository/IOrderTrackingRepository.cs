using Core.Model;
using Core.ModelDto.OrderTracking;

namespace Infrastructure.IRepository;

public interface IOrderTrackingRepository
{
    Task<PaginatedListModel<OrderTrackingResponseDto>> GetOrderTrackings(int pageNumber, OrderTrackingFilterDto searchModel);
    Task<List<OrderTrackingResponseDto>> GetDistinctOrderTrackings(OrderTrackingFilterDto searchModel);
    Task<OrderTrackingResponseDto> GetOrderTrackingById(int OrderTrackingId);
    Task<int> InsertOrderTracking(OrderTrackingRequestDto insertRequestModel);
    Task<int> UpdateOrderTracking(int OrderTrackingId, OrderTrackingRequestDto updateRequestModel);
    Task<int> DeleteOrderTracking(int OrderTrackingId, OrderTrackingRequestDto deleteRequestModel);
}
