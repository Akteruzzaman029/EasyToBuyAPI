namespace Core.ModelDto.Category
{
    public class EPSPGPaymentInitialResponsetDto
    {
        public string TransactionId { get; set; }=string.Empty;
        public string RedirectURL { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public int? ErrorCode { get; set; }
    }
}
