namespace Core.ModelDto.VehicleAvailability
{
    public class VehicleAvailabilityRequestDto
    {
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
        public DateTime AvailableDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
