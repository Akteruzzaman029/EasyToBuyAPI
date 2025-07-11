using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class ExpenditureModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int ExpenditureHeadId { get; set; }
        public int FileId { get; set; }
        public decimal Amount { get; set; }
    }
}
