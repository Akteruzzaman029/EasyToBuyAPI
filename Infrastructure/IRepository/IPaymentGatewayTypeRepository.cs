using Core.Model;
using Core.ModelDto.PaymentGatewayType;

namespace Infrastructure.IRepository;

public interface IPaymentGatewayTypeRepository
{
    Task<PaginatedListModel<PaymentGatewayTypeResponseDto>> GetCategories(int pageNumber, PaymentGatewayTypeFilterDto searchModel);
    Task<List<PaymentGatewayTypeResponseDto>> GetDistinctCategories(PaymentGatewayTypeFilterDto searchModel);
    Task<PaymentGatewayTypeResponseDto> GetPaymentGatewayTypeById(int PaymentGatewayTypeId);
    Task<PaymentGatewayTypeResponseDto> GetPaymentGatewayTypeByName(PaymentGatewayTypeRequestDto insertRequestModel);
    Task<int> InsertPaymentGatewayType(PaymentGatewayTypeRequestDto insertRequestModel);
    Task<int> UpdatePaymentGatewayType(int PaymentGatewayTypeId, PaymentGatewayTypeRequestDto updateRequestModel);
    Task<int> DeletePaymentGatewayType(int PaymentGatewayTypeId, PaymentGatewayTypeRequestDto deleteRequestModel);
}
