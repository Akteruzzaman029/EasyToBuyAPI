namespace Core.Model
{
    public class SlotAssignmentModel : BaseEntity
    {
        public int SlotId { get; set; }
        public int InstructorId { get; set; }
        public DateTime AvailableDate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
