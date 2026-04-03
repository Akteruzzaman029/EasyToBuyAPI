using Core.Model;
using Core.ModelDto.OrderFlowStage;

namespace Infrastructure.IRepository;

public interface IOrderFlowStageRepository
{
    Task<PaginatedListModel<OrderFlowStageResponseDto>> GetOrderFlowStages(int pageNumber, OrderFlowStageFilterDto searchModel);
    Task<List<OrderFlowStageResponseDto>> GetDistinctOrderFlowStages(OrderFlowStageFilterDto searchModel);
    Task<OrderFlowStageResponseDto> GetOrderFlowStageById(int OrderFlowStageId);
    Task<int> InsertOrderFlowStage(OrderFlowStageRequestDto insertRequestModel);
    Task<int> UpdateOrderFlowStage(int OrderFlowStageId, OrderFlowStageRequestDto updateRequestModel);
    Task<int> DeleteOrderFlowStage(int OrderFlowStageId, OrderFlowStageRequestDto deleteRequestModel);
}
