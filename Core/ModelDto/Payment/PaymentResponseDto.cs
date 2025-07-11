using Core.Model;

namespace Core.ModelDto.Payment
{
    public class PaymentResponseDto : PaymentModel
    {
        public string UserName { get; set; }=string.Empty;
        public string StudentIdNo { get; set; }=string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;
        public decimal DueAmount {
            get{
                return this.TotalAmount -this.PaidAmount;
            }
        }
        public string TransactionDateSt
        {
            get
            {
                return this.TransactionDate.ToString("dd MMM yyyy HH mm tt");
            }
        }
    }
}
