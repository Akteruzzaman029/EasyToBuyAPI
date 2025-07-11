using Core.Model;
using Core.ModelDto.Payment;

namespace Infrastructure.IRepository;

public interface IPaymentRepository
{
    Task<PaginatedListModel<PaymentResponseDto>> GetPayments(int pageNumber, PaymentFilterDto searchModel);
    Task<List<PaymentResponseDto>> GetDistinctPayments(int postId);
    Task<List<PaymentResponseDto>> GetPaymentByStudentId(int StudentID);
    Task<PaymentResponseDto> GetPaymentReceiptById(int PaymentId);
    Task<PaymentResponseDto> GetPaymentById(int PaymentId);
    Task<int> InsertPayment(PaymentRequestDto insertRequestModel);
    Task<int> UpdatePayment(int PaymentId, PaymentRequestDto updateRequestModel);
    Task<int> DeletePayment(int PaymentId, PaymentRequestDto deleteRequestModel);
}
