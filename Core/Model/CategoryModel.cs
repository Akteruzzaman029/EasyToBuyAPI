using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CategoryModel: BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int FileId { get; set; }
        public int ParentId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

