using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class AspNetRolesModel
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string NormalizedName { get; set; } = string.Empty;

        public string ConcurrencyStamp { get; set; } = string.Empty;
    }
}
