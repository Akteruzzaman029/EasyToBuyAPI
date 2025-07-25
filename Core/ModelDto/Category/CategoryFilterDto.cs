namespace Core.ModelDto.Category
{
    public class CategoryFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public int ParentId { get; set; } 
        public bool IsActive { get; set; }
    }
}
