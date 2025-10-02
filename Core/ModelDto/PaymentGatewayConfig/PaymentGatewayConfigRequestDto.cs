namespace Core.ModelDto.PaymentGatewayConfig
{
    public class PaymentGatewayConfigRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int PaymentGatewayTypeId { get; set; } 
        public int CompanyId { get; set; }
        public string MerchantId { get; set; } = string.Empty;
        public string StoreId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string HashKey { get; set; } = string.Empty;
        public string SuccessUrl { get; set; } = string.Empty;
        public string FailUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string SiteUrl { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
