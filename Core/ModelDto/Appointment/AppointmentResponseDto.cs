using Core.Model;

namespace Core.ModelDto.Appointment
{
    public class AppointmentResponseDto : AppointmentModel
    {
        public string Name { get; set; } = string.Empty;
        public string SlotName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
    }
}
