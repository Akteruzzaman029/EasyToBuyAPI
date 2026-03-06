using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class ProductCommentModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int FileId { get; set; } = 0;
    }
}
