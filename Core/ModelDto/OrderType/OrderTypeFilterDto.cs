namespace Core.ModelDto.OrderType
{
    public class OrderTypeFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public bool IsActive { get; set; }
    }
}
