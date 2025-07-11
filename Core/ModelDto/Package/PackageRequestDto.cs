namespace Core.ModelDto.Package
{
    public class PackageRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalLessons { get; set; }
        public decimal Rate { get; set; }
        public int FileId { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
