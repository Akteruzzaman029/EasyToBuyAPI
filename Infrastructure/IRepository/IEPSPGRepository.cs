using Core.Model;
using Core.ModelDto.Category;
using Core.ModelDto.EPSPG;
using Core.ModelDto.PaymentGatewayConfig;

namespace Infrastructure.IRepository;

public interface IEPSPGRepository
{
    Task<string> GetPGAuthentication(EPSPGLoginRequestDto pGLoginRequestDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto);
    Task<EPSPGPaymentInitialResponsetDto> EPSPGPaymentInitialize(EPSPGPaymentRequestDto ePSPGCheckPaymentStatusRequestDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto);
    Task<EPSPGPaymentResponseDto> CheckPaymentStatus(EPSPGCheckPaymentStatusRequestDto ePSPGCheckPaymentStatusRequestDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto);
    Task<EPSPGPaymentResponseDto> CheckMerchantPaymentReferenceStatus(EPSPGCheckPaymentStatusRequestDto ePSPGCheckPaymentStatusRequestDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto);
    EPSFinalResponseDto FinalResponseGenerator(EPSFinalResponseDto ePSFinalResponseDto, int status, string errorMessage = null);
    string UrlGenerator(EPSFinalResponseDto ePSFinalResponseDto, string url);
}
