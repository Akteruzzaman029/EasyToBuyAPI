namespace Core.ModelDto.MeasurementUnit
{
    public class MeasurementUnitFilterDto
    {
        public int CompanyId { get; set; }
        public string UnitName { get; set; }
        public int? UnitType { get; set; } // tinyint => byte
        public bool IsActive { get; set; }
    }
}