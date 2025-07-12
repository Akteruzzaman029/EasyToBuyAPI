using Core.Model;
using Core.ModelDto.OrderPayment;

namespace Infrastructure.IRepository;

public interface IOrderPaymentRepository
{
    Task<PaginatedListModel<OrderPaymentResponseDto>> GetOrderPayments(int pageNumber, OrderPaymentFilterDto searchModel);
    Task<List<OrderPaymentResponseDto>> GetDistinctOrderPayments(OrderPaymentFilterDto searchModel);
    Task<OrderPaymentResponseDto> GetOrderPaymentById(int OrderPaymentId);
    Task<List<OrderPaymentResponseDto>> GetOrderPaymentsByName(OrderPaymentRequestDto insertRequestModel);
    Task<int> InsertOrderPayment(OrderPaymentRequestDto insertRequestModel);
    Task<int> UpdateOrderPayment(int OrderPaymentId, OrderPaymentRequestDto updateRequestModel);
    Task<int> DeleteOrderPayment(int OrderPaymentId, OrderPaymentRequestDto deleteRequestModel);
}
