using Core.Model;

namespace Core.ModelDto.VehicleAvailability
{
    public class VehicleAvailabilityResponseDto : VehicleAvailabilityModel
    {
        public string SlotName { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
