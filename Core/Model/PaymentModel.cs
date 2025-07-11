namespace Core.Model
{
    public class PaymentModel : BaseEntity
    {
        public string UserId { get; set; } =string.Empty;
        public int PackageId { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; } 
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; } =string.Empty;
        public string ReceiptNo { get; set; } = string.Empty;
    }
}
