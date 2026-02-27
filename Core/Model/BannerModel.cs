using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class BannerModel: BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int FileId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

