using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class PostModel : BaseEntity
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public string ShortName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? FileId { get; set; }
    }
}
