namespace Core.Model
{
    public class PackageModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalLessons { get; set; }
        public decimal Rate { get; set; }
        public int FileId { get; set; }

    }
}
