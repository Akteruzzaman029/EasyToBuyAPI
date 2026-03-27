namespace Core.ModelDto.CustomCategory
{
    public class CustomCategoryFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public int CategoryId { get; set; } 
        public bool IsActive { get; set; }
    }
}
