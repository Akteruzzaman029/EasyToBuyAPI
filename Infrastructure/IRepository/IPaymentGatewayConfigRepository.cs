using Core.Model;
using Core.ModelDto.PaymentGatewayConfig;

namespace Infrastructure.IRepository;

public interface IPaymentGatewayConfigRepository
{
    Task<PaginatedListModel<PaymentGatewayConfigResponseDto>> GetCategories(int pageNumber, PaymentGatewayConfigFilterDto searchModel);
    Task<List<PaymentGatewayConfigResponseDto>> GetDistinctCategories(PaymentGatewayConfigFilterDto searchModel);
    Task<PaymentGatewayConfigResponseDto> GetPaymentGatewayConfigById(int PaymentGatewayConfigId);
    Task<PaymentGatewayConfigResponseDto> GetPaymentGatewayConfigByName(PaymentGatewayConfigRequestDto insertRequestModel);
    Task<int> InsertPaymentGatewayConfig(PaymentGatewayConfigRequestDto insertRequestModel);
    Task<int> UpdatePaymentGatewayConfig(int PaymentGatewayConfigId, PaymentGatewayConfigRequestDto updateRequestModel);
    Task<int> DeletePaymentGatewayConfig(int PaymentGatewayConfigId, PaymentGatewayConfigRequestDto deleteRequestModel);
}
