using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class ProductModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public int PackTypeId { get; set; }
        public int ModelNo { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal VAT { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool? IsConsider { get; set; }       // Nullable in SQL
        public bool? IsBarCode { get; set; }        // Nullable in SQL
        public int FileId { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public bool IsFixedAmount { get; set; }
        public decimal Discount { get; set; } = 0;
    }
}
