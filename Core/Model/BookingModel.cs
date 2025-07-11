namespace Core.Model
{
    public class BookingModel : BaseEntity
    {
        public int StudentId { get; set; }
        public int SlotId { get; set; }
        public int InstructorId { get; set; }
        public int VehicleId { get; set; }
        public DateTime ClassDate { get; set; }
        public int Status { get; set; }
    }
}
