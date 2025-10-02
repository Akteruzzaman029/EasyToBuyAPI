using Core.Model;

namespace Core.ModelDto.PaymentGatewayConfig
{
    public class PaymentGatewayConfigResponseDto : PaymentGatewayConfigModel
    {
        public string PaymentGatewayTypeName { get; set; } = string.Empty;
    }
}
