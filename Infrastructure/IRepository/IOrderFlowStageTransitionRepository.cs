using Core.Model;
using Core.ModelDto.OrderFlowStageTransition;

namespace Infrastructure.IRepository;

public interface IOrderFlowStageTransitionRepository
{
    Task<PaginatedListModel<OrderFlowStageTransitionResponseDto>> GetOrderFlowStageTransitions(int pageNumber, OrderFlowStageTransitionFilterDto searchModel);
    Task<List<OrderFlowStageTransitionResponseDto>> GetDistinctOrderFlowStageTransitions(OrderFlowStageTransitionFilterDto searchModel);
    Task<OrderFlowStageTransitionResponseDto> GetOrderFlowStageTransitionById(int OrderFlowStageTransitionId);
    Task<int> InsertOrderFlowStageTransition(OrderFlowStageTransitionRequestDto insertRequestModel);
    Task<int> UpdateOrderFlowStageTransition(int OrderFlowStageTransitionId, OrderFlowStageTransitionRequestDto updateRequestModel);
    Task<int> DeleteOrderFlowStageTransition(int OrderFlowStageTransitionId, OrderFlowStageTransitionRequestDto deleteRequestModel);
}
