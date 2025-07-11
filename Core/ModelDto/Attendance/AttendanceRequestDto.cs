namespace Core.ModelDto.Attendance
{
    public class AttendanceRequestDto
    {
        public int? BookingId { get; set; }
        public bool Attended { get; set; }
        public string? MarkBy { get; set; }
        public DateTime MarkDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
