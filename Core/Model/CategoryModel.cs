using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CategoryModel: BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int ParentId { get; set; }
    }
}
