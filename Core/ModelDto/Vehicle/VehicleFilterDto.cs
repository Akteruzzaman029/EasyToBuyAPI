namespace Core.ModelDto.Vehicle
{
    public class VehicleFilterDto
    {
        public string Model { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public int FuelType { get; set; } // Consider using enum
        public int TransmissionType { get; set; } // Consider using enum
        public int Status { get; set; } // Consider using enum
        public bool IsActive { get; set; }
    }
}
