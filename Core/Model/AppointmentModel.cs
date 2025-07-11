namespace Core.Model
{
    public class AppointmentModel : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public int SlotId { get; set; }

        public int InstructorId { get; set; }

        public int Status { get; set; }

        public DateTime RequestedAt { get; set; }

        public string ApprovedBy { get; set; } = string.Empty;
    }
}
