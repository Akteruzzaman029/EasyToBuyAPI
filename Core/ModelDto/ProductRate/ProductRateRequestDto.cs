namespace Core.ModelDto.ProductRate
{
    public class ProductRateRequestDto
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Rate { get; set; } = 0;
        public int TypeId { get; set; } = 0;
        public int FileId { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
    }
}
