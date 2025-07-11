namespace Core.ModelDto.Booking
{
    public class BookingRequestDto
    {
        public int StudentId { get; set; }
        public int slotId { get; set; }
        public DateTime ClassDate { get; set; }
        public string SlotName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Status { get; set; }
        public bool IsRepeat { get; set; } = false;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
