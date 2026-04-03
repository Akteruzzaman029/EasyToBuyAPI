using Core.Model;
using Core.ModelDto.OrderDeliveryChargeDetail;

namespace Infrastructure.IRepository;

public interface IOrderDeliveryChargeDetailRepository
{
    Task<PaginatedListModel<OrderDeliveryChargeDetailResponseDto>> GetOrderDeliveryChargeDetails(int pageNumber, OrderDeliveryChargeDetailFilterDto searchModel);
    Task<List<OrderDeliveryChargeDetailResponseDto>> GetDistinctOrderDeliveryChargeDetails(OrderDeliveryChargeDetailFilterDto searchModel);
    Task<OrderDeliveryChargeDetailResponseDto> GetOrderDeliveryChargeDetailById(int OrderDeliveryChargeDetailId);
    Task<int> InsertOrderDeliveryChargeDetail(OrderDeliveryChargeDetailRequestDto insertRequestModel);
    Task<int> UpdateOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, OrderDeliveryChargeDetailRequestDto updateRequestModel);
    Task<int> DeleteOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, OrderDeliveryChargeDetailRequestDto deleteRequestModel);
}
