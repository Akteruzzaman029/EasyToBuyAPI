namespace Core.ModelDto.VehicleAvailability
{
    public class VehicleAvailabilityFilterDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
