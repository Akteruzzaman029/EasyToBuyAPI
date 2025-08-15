namespace Core.ModelDto.Address
{
    public class AddressFilterDto
    {
        public string? UserId { get; set; }
        public string? PickerName { get; set; }
        public string? PickerNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? Building { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
