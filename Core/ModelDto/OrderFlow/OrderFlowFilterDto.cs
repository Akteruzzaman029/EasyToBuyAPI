namespace Core.ModelDto.OrderFlow
{
    public class OrderFlowFilterDto
    {
        public int CompanyId { get; set; }
        public int OderTypeId { get; set; } // DB typo preserved
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
