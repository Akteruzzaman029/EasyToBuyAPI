using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CustomCategoryConfigModel : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int CompanyId { get; set; }
    }
}

