namespace Core.ModelDto.PaymentGatewayType
{
    public class PaymentGatewayTypeFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public bool IsActive { get; set; }
    }
}
