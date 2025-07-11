using Core.Model;

namespace Core.ModelDto.Payment
{
    public class PaymentReceiptResponseDto : PaymentModel
    {
        public string UserName { get; set; }=string.Empty;
        public string PackageName { get; set; } = string.Empty;
    }
}
