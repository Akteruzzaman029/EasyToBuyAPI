namespace Core.ModelDto.SlotAssignment
{
    public class SlotAssignmentRequestDto
    {
        public int SlotId { get; set; }
        public int InstructorId { get; set; }
        public DateTime AvailableDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
