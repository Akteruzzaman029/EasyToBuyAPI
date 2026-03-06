namespace Core.ModelDto.ProductRate
{
    public class ProductRateFilterDto
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Rate { get; set; } = 0;
        public bool IsActive { get; set; }

    }
}
