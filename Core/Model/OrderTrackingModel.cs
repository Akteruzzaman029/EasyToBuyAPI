using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderTrackingModel: BaseEntity
    {
        public int SalesOrderId { get; set; }
        public int OrderFlowStageId { get; set; }
        public int? TrackingStatus { get; set; }
        public string? TrackingMessage { get; set; }
        public string? LocationName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public bool IsCustomerVisible { get; set; }
    }
}

