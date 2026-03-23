namespace Core.ModelDto.Product
{
    public class ProductFilterDto
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public int PackTypeId { get; set; }
        public string ModelNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string BrandIds { get; set; } = string.Empty;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Rating { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; }

    }
}
