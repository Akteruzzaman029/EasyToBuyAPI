namespace Core.ModelDto.LessonProgres
{
    public class LessonProgresDto
    {
        public int BookingId { get; set; }
        public int Status { get; set; }
        public DateTime OldClassDate { get; set; }
        public int OldSlotId { get; set; }
        public DateTime NewClassDate { get; set; }
        public int NewSlotId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string LessonTitle { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public int? ProgressPercentage { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
