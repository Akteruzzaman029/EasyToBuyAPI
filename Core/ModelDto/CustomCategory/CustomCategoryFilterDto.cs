namespace Core.ModelDto.CustomCategory
{
    public class CustomCategoryFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int CustomCategoryConfigId { get; set; } 
        public int CompanyId { get; set; } 
        public int CategoryId { get; set; } 
        public bool IsActive { get; set; }
    }
}
