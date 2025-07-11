using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class UserFileModel: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int FileId { get; set; }
    }
}
