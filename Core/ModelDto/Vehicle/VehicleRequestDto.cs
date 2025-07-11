namespace Core.ModelDto.Vehicle
{
    public class VehicleRequestDto
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int FuelType { get; set; } // Consider using enum
        public int TransmissionType { get; set; } // Consider using enum
        public int Status { get; set; } // Consider using enum
        public DateTime? LastServicedAt { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
