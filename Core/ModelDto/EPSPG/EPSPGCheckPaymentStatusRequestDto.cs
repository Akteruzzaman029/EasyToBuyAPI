using Core.Model;

namespace Core.ModelDto.EPSPG;

public class EPSPGCheckPaymentStatusRequestDto
{
    public string epsTransactionId { get; set; } = string.Empty;
    public string paymentReferance { get; set; } = string.Empty;
    public string merchantId { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public int transactionTypeId { get; set; }
    public string Token { get; set; } = string.Empty;
}
