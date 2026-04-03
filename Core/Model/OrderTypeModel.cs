using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderTypeModel: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }
}

