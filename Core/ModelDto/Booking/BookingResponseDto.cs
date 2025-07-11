using Core.Model;
using Microsoft.VisualBasic.FileIO;
using static Core.BaseEnum;

namespace Core.ModelDto.Booking
{
    public class BookingResponseDto : BookingModel
    {
        public string SlotName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string StatusName
        {
            get
            {
                return Enum.IsDefined(typeof(AppointmentStatusEnum), Status)
                    ? ((AppointmentStatusEnum)Status).ToString()
                    : "none";
            }
        }

    }
}
