using Core.Model;

namespace Core.ModelDto.SlotAssignment
{
    public class SlotAssignmentResponseDto : SlotAssignmentModel
    {
        public string SlotName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
