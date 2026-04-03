using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderFlowModel: BaseEntity
    {
        public int CompanyId { get; set; }
        public int OderTypeId { get; set; } // DB typo preserved
        public string? Name { get; set; }
        public bool IsDefault { get; set; }
    }
}

