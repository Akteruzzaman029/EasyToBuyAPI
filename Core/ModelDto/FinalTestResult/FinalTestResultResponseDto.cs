using Core.Model;

namespace Core.ModelDto.FinalTestResult
{
    public class FinalTestResultResponseDto : FinalTestResultModel
    {
        public string SlotName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
