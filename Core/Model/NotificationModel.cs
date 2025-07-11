using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class NotificationModel : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Messaage { get; set; } = string.Empty;
        public bool Seen { get; set; } = false;


    }
}
