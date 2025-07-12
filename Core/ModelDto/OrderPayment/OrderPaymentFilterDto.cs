namespace Core.ModelDto.OrderPayment
{
    public class OrderPaymentFilterDto
    {
        public int CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrderNo { get; set; } = string.Empty;               // Foreign key to Order table
        public string? Reference { get; set; }             // Transaction reference ID or code
        public int PaymentStatus { get; set; }             // Can be mapped to an enum
        public bool IsActive { get; set; }
    }
}
