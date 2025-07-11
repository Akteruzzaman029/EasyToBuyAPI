namespace Core.ModelDto.Booking
{
    public class BookingAssignRequestDto
    {
        public int StudentId { get; set; }
        public int slotId { get; set; }
        public int PackageId { get; set; }
        public int InstructorId { get; set; }
        public int VehicleId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime PackageStartDate { get; set; }
        public List<SlotDto> Slots { get; set; } = new List<SlotDto>();
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SlotDto
    {
        public DateTime Date { get; set; }
        public string Day { get; set; } = string.Empty;
        public string SlotName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Status { get; set; }
        public int StatusName { get; set; }
    }
}
