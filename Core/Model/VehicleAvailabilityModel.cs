namespace Core.Model
{
    public class VehicleAvailabilityModel : BaseEntity
    {
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
        public DateTime AvailableDate { get; set; }
    }
}
