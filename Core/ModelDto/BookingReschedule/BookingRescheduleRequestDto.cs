namespace Core.ModelDto.BookingReschedule
{
    public class BookingRescheduleRequestDto
    {
        public int BookingId { get; set; }
        public DateTime OldClassDate { get; set; }
        public DateTime NewClassDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
