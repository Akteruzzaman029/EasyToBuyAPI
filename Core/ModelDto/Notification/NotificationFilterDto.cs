namespace Core.ModelDto.Notification
{
    public class NotificationFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Messaage { get; set; } = string.Empty;
        public bool Seen { get; set; } = false;
        public bool IsActive { get; set; }
    }
}
