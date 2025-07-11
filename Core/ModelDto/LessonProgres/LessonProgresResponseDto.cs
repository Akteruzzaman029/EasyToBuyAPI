using Core.Model;

namespace Core.ModelDto.LessonProgres
{
    public class LessonProgresResponseDto : LessonProgresModel
    {
        public string SlotName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
