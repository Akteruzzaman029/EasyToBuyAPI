using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CustomCategoryModel : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int FileId { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }
}

