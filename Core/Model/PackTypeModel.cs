using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class PackTypeModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
