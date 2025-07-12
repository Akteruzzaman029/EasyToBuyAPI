namespace Core.Model
{
    public class OrderPaymentModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int? OrderId { get; set; }                  // Foreign key to Order table
        public string? PaymentMethod { get; set; }         // e.g., Card, Cash, MobilePay
        public string? Reference { get; set; }             // Transaction reference ID or code
        public int PaymentStatus { get; set; }             // Can be mapped to an enum
        public decimal PaidAmount { get; set; }
    }
}
