namespace Core.ModelDto.Notification
{
    public class NotificationRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Messaage { get; set; } = string.Empty;
        public bool Seen { get; set; } = false;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
