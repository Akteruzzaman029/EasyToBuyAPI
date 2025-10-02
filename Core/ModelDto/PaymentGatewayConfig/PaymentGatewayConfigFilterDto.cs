namespace Core.ModelDto.PaymentGatewayConfig
{
    public class PaymentGatewayConfigFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public int PaymentGatewayTypeId { get; set; } 
        public bool IsActive { get; set; }
    }
}
