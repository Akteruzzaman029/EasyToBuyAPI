using Core.Model;
using Core.ModelDto.SalesOrder;

namespace Infrastructure.IRepository;

public interface ISalesOrderRepository
{
    Task<PaginatedListModel<SalesOrderResponseDto>> GetSalesOrders(int pageNumber, SalesOrderFilterDto searchModel);
    Task<List<SalesOrderResponseDto>> GetDistinctSalesOrders(SalesOrderFilterDto searchModel);
    Task<SalesOrderResponseDto> GetSalesOrderById(int SalesOrderId);
    Task<int> InsertSalesOrder(SalesOrderRequestDto insertRequestModel);
    Task<int> UpdateSalesOrder(int SalesOrderId, SalesOrderRequestDto updateRequestModel);
    Task<int> DeleteSalesOrder(int SalesOrderId, SalesOrderRequestDto deleteRequestModel);
}

