using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class LessonProgresModel : BaseEntity
    {
        public int? BookingId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public int? ProgressPercentage { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
