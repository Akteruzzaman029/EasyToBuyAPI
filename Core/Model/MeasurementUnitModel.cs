using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class MeasurementUnitModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public int? UnitType { get; set; } // tinyint => byte
        public string Note { get; set; }
        public bool? IsRound { get; set; }
        public bool? IsSmallUnit { get; set; }
        public string SymbolInBangla { get; set; }
    }
}
