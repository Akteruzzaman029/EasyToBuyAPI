using Core.Model;
using Core.ModelDto.OrderFlow;

namespace Infrastructure.IRepository;

public interface IOrderFlowRepository
{
    Task<PaginatedListModel<OrderFlowResponseDto>> GetOrderFlows(int pageNumber, OrderFlowFilterDto searchModel);
    Task<List<OrderFlowResponseDto>> GetDistinctOrderFlows(OrderFlowFilterDto searchModel);
    Task<OrderFlowResponseDto> GetOrderFlowById(int OrderFlowId);
    Task<int> InsertOrderFlow(OrderFlowRequestDto insertRequestModel);
    Task<int> UpdateOrderFlow(int OrderFlowId, OrderFlowRequestDto updateRequestModel);
    Task<int> DeleteOrderFlow(int OrderFlowId, OrderFlowRequestDto deleteRequestModel);
}
