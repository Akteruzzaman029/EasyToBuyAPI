using Core.Model;
using Core.ModelDto.OrderType;

namespace Infrastructure.IRepository;

public interface IOrderTypeRepository
{
    Task<PaginatedListModel<OrderTypeResponseDto>> GetOrderTypes(int pageNumber, OrderTypeFilterDto searchModel);
    Task<List<OrderTypeResponseDto>> GetDistinctOrderTypes(OrderTypeFilterDto searchModel);
    Task<OrderTypeResponseDto> GetOrderTypeById(int OrderTypeId);
    Task<int> InsertOrderType(OrderTypeRequestDto insertRequestModel);
    Task<int> UpdateOrderType(int OrderTypeId, OrderTypeRequestDto updateRequestModel);
    Task<int> DeleteOrderType(int OrderTypeId, OrderTypeRequestDto deleteRequestModel);
}
