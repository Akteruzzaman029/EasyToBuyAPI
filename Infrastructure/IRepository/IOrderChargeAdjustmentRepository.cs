using Core.Model;
using Core.ModelDto.OrderChargeAdjustment;

namespace Infrastructure.IRepository;

public interface IOrderChargeAdjustmentRepository
{
    Task<PaginatedListModel<OrderChargeAdjustmentResponseDto>> GetOrderChargeAdjustments(int pageNumber, OrderChargeAdjustmentFilterDto searchModel);
    Task<List<OrderChargeAdjustmentResponseDto>> GetDistinctOrderChargeAdjustments(OrderChargeAdjustmentFilterDto searchModel);
    Task<OrderChargeAdjustmentResponseDto> GetOrderChargeAdjustmentById(int OrderChargeAdjustmentId);
    Task<int> InsertOrderChargeAdjustment(OrderChargeAdjustmentRequestDto insertRequestModel);
    Task<int> UpdateOrderChargeAdjustment(int OrderChargeAdjustmentId, OrderChargeAdjustmentRequestDto updateRequestModel);
    Task<int> DeleteOrderChargeAdjustment(int OrderChargeAdjustmentId, OrderChargeAdjustmentRequestDto deleteRequestModel);
}
