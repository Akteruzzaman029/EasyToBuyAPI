namespace Core.ModelDto.EPSPG;

public class EPSFinalResponseDto
{
    public int CompanyId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string MerchantTransactionId { get; set; } = string.Empty;
    public string EPSTransactionId { get; set; } = string.Empty;
    public int? ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
