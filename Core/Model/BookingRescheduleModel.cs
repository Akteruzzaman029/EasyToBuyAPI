using System;

namespace Core.Model
{
    public class BookingRescheduleModel : BaseEntity
    {
        public int BookingId { get; set; }
        public DateTime OldClassDate { get; set; }
        public DateTime NewClassDate { get; set; }
        public string Reason { get; set; }=string.Empty;
    }
}