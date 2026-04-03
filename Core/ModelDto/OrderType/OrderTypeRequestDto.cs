namespace Core.ModelDto.OrderType
{
    public class OrderTypeRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
