namespace Core.ModelDto.Product
{
    public class ProductRequestDto
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public int PackTypeId { get; set; }
        public string ModelNo { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal VAT { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool? IsConsider { get; set; }
        public bool? IsBarCode { get; set; }
        public int FileId { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public bool IsFixedAmount { get; set; }
        public decimal Discount { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
    }
}
