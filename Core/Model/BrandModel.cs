using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class BrandModel: BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int FileId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

