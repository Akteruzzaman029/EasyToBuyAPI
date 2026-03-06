namespace Core.ModelDto.Brand
{
    public class BrandFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public bool IsActive { get; set; }
    }
}
