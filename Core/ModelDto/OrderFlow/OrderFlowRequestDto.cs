namespace Core.ModelDto.OrderFlow
{
    public class OrderFlowRequestDto
    {
        public int CompanyId { get; set; }
        public int OderTypeId { get; set; }
        public string? Name { get; set; }
        public bool IsDefault { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
