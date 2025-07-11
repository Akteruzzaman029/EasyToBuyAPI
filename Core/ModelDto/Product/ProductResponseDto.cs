using Core.Model;

namespace Core.ModelDto.Product
{
    public class ProductResponseDto : ProductModel
    {

        public string CompanyName { get; set; }=string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string SubCategoryName { get; set; } = string.Empty;
        public string MeasurementUnitName { get; set; } = string.Empty;
        public string PackTypeName { get; set; } = string.Empty;

    }
}
