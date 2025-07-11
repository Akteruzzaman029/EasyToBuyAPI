namespace Core.ModelDto.Appointment
{
    public class AppointmentFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string SlotName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
