using Core.Model;

namespace Core.ModelDto.EPSPG;

public class EPSPGTokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime? ExpireDate { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int? ErrorCode { get; set; }
}