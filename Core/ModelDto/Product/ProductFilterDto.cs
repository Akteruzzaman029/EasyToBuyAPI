namespace Core.ModelDto.Product
{
    public class ProductFilterDto
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public int PackTypeId { get; set; }
        public int ModelNo { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
