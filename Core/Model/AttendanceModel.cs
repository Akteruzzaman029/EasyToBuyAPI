using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class AttendanceModel : BaseEntity
    {
        public int? BookingId { get; set; }
        public bool Attended { get; set; }
        public string? MarkBy { get; set; }
        public DateTime MarkDate { get; set; }
    }
}
